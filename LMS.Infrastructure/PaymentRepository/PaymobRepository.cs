using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Data_.Helper;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LMS.Infrastructure
{
    public class PaymobRepository : GenericRepositoryAsync<Payments>, IPaymobRepository
    {
        private readonly PaymobSetting _paymobSetting;
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly AppDbContext dbContext;
        private readonly ISubscriptionsRepository subscriptionsRepository;
        private readonly DbSet<Payments> PaymentsRepo;

        public PaymobRepository(
            IOptions<PaymobSetting> paymobSetting,
            IEnrollmentRepository enrollmentRepository,
            AppDbContext dbContext,
            ISubscriptionsRepository subscriptionsRepository,
            AppDbContext appDbContext) : base(appDbContext)
        {
            _paymobSetting = paymobSetting.Value;
            this.enrollmentRepository = enrollmentRepository;
            this.dbContext = dbContext;
            this.subscriptionsRepository = subscriptionsRepository;
            PaymentsRepo = dbContext.Set<Payments>();
        }

        public async Task<(Subscriptions Subscription, string RedirectUrl)> ProcessPaymentAsync(
            int subscribeId, string paymentMethod)
        {
            var subscribe = await dbContext.Subscriptions
                .Include(s => s.Plan)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == subscribeId);

            if (subscribe == null)
                throw new KeyNotFoundException($"Subscription with ID {subscribeId} not found.");

            if (subscribe.User == null)
                throw new InvalidOperationException("User not found.");

            if (subscribe.Status != SubscriptionStatusEnum.Pending)
                throw new InvalidOperationException("Subscription is not in Pending state.");

            var student = subscribe.User;

            string secretKey = _paymobSetting.SecretKey
                ?? throw new ArgumentException("Paymob secret key not configured");

            string publicKey = _paymobSetting.publicKey
                ?? throw new ArgumentException("Paymob public key not configured");

            int specialReference = RandomNumberGenerator.GetInt32(1000000, 9999999) + subscribeId;

            var discount = subscribe.Discount / 100m;
            var priceAfterDiscount = subscribe.Plan.Price * (1 - discount);
            var amountCents = (int)(priceAfterDiscount * 100);

            var billingData = new
            {
                apartment = "N/A",
                first_name = student.UserName ?? "Guest",
                last_name = student.UserName ?? "User",
                street = "N/A",
                building = "N/A",
                phone_number = "01000000000",
                country = "EG",
                email = student.Email,
                floor = "N/A",
                state = "N/A",
                city = "N/A"
            };

            var integrationId = int.Parse(DetermineIntegrationId(paymentMethod));

            var payload = new
            {
                amount = amountCents,
                currency = "EGP",
                payment_methods = new[] { integrationId },
                billing_data = billingData,
                merchant_id = int.Parse(_paymobSetting.MerchantId),
                items = new[]
                {
                    new
                    {
                        name = subscribe.Plan.Name,
                        amount = amountCents,
                        description = $"Subscription Payment for Plan: {subscribe.Plan.Name}",
                        quantity = 1
                    }
                },
                customer = new
                {
                    first_name = billingData.first_name,
                    last_name = billingData.last_name,
                    email = billingData.email
                },
                extras = new
                {
                    subscribeId = subscribe.Id,
                    customerId = student.Id
                },
                special_reference = specialReference,
                expiration = 3600,
                merchant_order_id = specialReference.ToString()
            };

            var httpClient = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post,
                "https://accept.paymob.com/v1/intention/");
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Token", secretKey);
            requestMessage.Content = JsonContent.Create(payload);

            var response = await httpClient.SendAsync(requestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Paymob API failed: {response.StatusCode} - {responseContent}");

            var resultJson = JsonDocument.Parse(responseContent);
            var clientSecret = resultJson.RootElement
                .GetProperty("client_secret").GetString();

            var payment = new Payments
            {
                Amount = amountCents / 100m,
                PaymentMethod = Enum.Parse<PaymentMethodEnum>(paymentMethod, true),
                PaymentStatus = PaymentStatusEnum.Pending,
                PaymentDate = DateTime.UtcNow,
                TransactionId = specialReference.ToString(),
                SubscriptionId = subscribe.Id,
                Currency = CurrencyEnum.EGP,
                UserId = student.Id,
            };

            await dbContext.Payments.AddAsync(payment);
            await dbContext.SaveChangesAsync();

            string redirectUrl = $"https://accept.paymob.com/unifiedcheckout/" +
                                 $"?publicKey={publicKey}&clientSecret={clientSecret}";

            return (subscribe, redirectUrl);
        }

        public async Task<Subscriptions> UpdateSubscriptionsSuccess(string specialReference)
        {
            var payment = await dbContext.Payments
                .Include(p => p.Subscription)
                    .ThenInclude(s => s.Plan)
                .FirstOrDefaultAsync(p => p.TransactionId == specialReference);

            if (payment == null)
                throw new KeyNotFoundException($"Payment {specialReference} not found.");

            payment.PaymentStatus = PaymentStatusEnum.Completed;
            payment.UpdatedAt = DateTime.UtcNow;

            payment.Subscription.Status = SubscriptionStatusEnum.Success;
            payment.Subscription.IsActive = true;
            payment.Subscription.StartDate = DateOnly.FromDateTime(DateTime.UtcNow);
            payment.Subscription.EndDate = DateOnly.FromDateTime(
                DateTime.UtcNow.AddMonths(payment.Subscription.Plan.DurationInMonth)
            );

            await dbContext.SaveChangesAsync();
            return payment.Subscription;
        }

        public async Task<Subscriptions> UpdateSubscriptionsFailed(string specialReference)
        {
            var payment = await dbContext.Payments
                .Include(p => p.Subscription)
                .FirstOrDefaultAsync(p => p.TransactionId == specialReference);

            if (payment == null)
                throw new KeyNotFoundException($"Payment with transaction ID {specialReference} not found.");

            var subscribe = await dbContext.Subscriptions
                .Include(e => e.User)
                .Include(e => e.Plan)
                .FirstOrDefaultAsync(e => e.Id == payment.SubscriptionId);

            if (subscribe == null)
                throw new KeyNotFoundException($"Subscription with ID {payment.SubscriptionId} not found.");

            subscribe.IsActive = false;
            payment.PaymentStatus = PaymentStatusEnum.Refunded;

            await dbContext.SaveChangesAsync();
            return payment.Subscription;
        }

        public Task<List<Payments>> MyHistory(string userId)
        {
            return PaymentsRepo
                .Where(p => p.UserId == userId)
                .Include(a => a.user)
                .Include(p => p.Subscription)
                .ThenInclude(s => s.Plan)
                .OrderByDescending(p => p.PaymentDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(Subscriptions, string)> ProcessRenewalPaymentAsync(
            int subscribeId, int newPlanId, string paymentMethod)
        {
            var subscribe = await dbContext.Subscriptions
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == subscribeId);

            if (subscribe == null)
                throw new KeyNotFoundException($"Subscription with ID {subscribeId} not found.");

            if (subscribe.User == null)
                throw new InvalidOperationException("User not found.");

            var newPlan = await dbContext.Plans
                .FirstOrDefaultAsync(p => p.Id == newPlanId)
                ?? throw new KeyNotFoundException($"Plan with ID {newPlanId} not found.");

            string secretKey = _paymobSetting.SecretKey
                ?? throw new ArgumentException("Paymob secret key not configured");

            string publicKey = _paymobSetting.publicKey
                ?? throw new ArgumentException("Paymob public key not configured");

            var student = subscribe.User;

            int specialReference = RandomNumberGenerator.GetInt32(1000000, 9999999) + subscribeId;

            var amountCents = (int)(newPlan.Price * 100);

            var billingData = new
            {
                apartment = "N/A",
                first_name = student.UserName ?? "Guest",
                last_name = student.UserName ?? "User",
                street = "N/A",
                building = "N/A",
                phone_number = "01000000000",
                country = "EG",
                email = student.Email,
                floor = "N/A",
                state = "N/A",
                city = "N/A"
            };

            var integrationId = int.Parse(DetermineIntegrationId(paymentMethod));

            var payload = new
            {
                amount = amountCents,
                currency = "EGP",
                payment_methods = new[] { integrationId },
                billing_data = billingData,
                merchant_id = int.Parse(_paymobSetting.MerchantId),
                items = new[]
                {
                    new
                    {
                        name = newPlan.Name,
                        amount = amountCents,
                        description = $"Subscription Renewal for Plan: {newPlan.Name}",
                        quantity = 1
                    }
                },
                customer = new
                {
                    first_name = billingData.first_name,
                    last_name = billingData.last_name,
                    email = billingData.email
                },
                extras = new
                {
                    subscribeId = subscribe.Id,
                    customerId = student.Id,
                    newPlanId = newPlan.Id
                },
                special_reference = specialReference,
                expiration = 3600,
                merchant_order_id = specialReference.ToString()
            };

            var httpClient = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post,
                "https://accept.paymob.com/v1/intention/");
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Token", secretKey);
            requestMessage.Content = JsonContent.Create(payload);

            var response = await httpClient.SendAsync(requestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Paymob API failed: {response.StatusCode} - {responseContent}");

            var resultJson = JsonDocument.Parse(responseContent);
            var clientSecret = resultJson.RootElement
                .GetProperty("client_secret").GetString();

            var payment = new Payments
            {
                Amount = amountCents / 100m,
                PaymentMethod = Enum.Parse<PaymentMethodEnum>(paymentMethod, true),
                PaymentStatus = PaymentStatusEnum.Pending,
                PaymentDate = DateTime.UtcNow,
                TransactionId = specialReference.ToString(),
                SubscriptionId = subscribe.Id,
                Currency = CurrencyEnum.EGP,
                UserId = student.Id,
            };

            await dbContext.Payments.AddAsync(payment);
            await dbContext.SaveChangesAsync();

            string redirectUrl = $"https://accept.paymob.com/unifiedcheckout/" +
                                 $"?publicKey={publicKey}&clientSecret={clientSecret}";

            return (subscribe, redirectUrl);
        }

        public async Task<List<Payments>> GetAllPaymentCompleted()
        {
            return await PaymentsRepo
                .Where(a => a.PaymentStatus == PaymentStatusEnum.Completed)
                .ToListAsync();
        }

        private string DetermineIntegrationId(string paymentMethod)
        {
            return paymentMethod?.ToLower() switch
            {
                "onlinecard" or "visa" or "card" => _paymobSetting.CardIntegrationId
                    ?? throw new ArgumentException("Card integration ID not configured"),
                "wallet" => _paymobSetting.MobileIntegrationId
                    ?? throw new ArgumentException("Wallet integration ID not configured"),
                _ => throw new ArgumentException($"Invalid payment method: {paymentMethod}")
            };
        }

        public string ComputeHmacSHA512(string data, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using var hmac = new HMACSHA512(keyBytes);
            var hash = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}