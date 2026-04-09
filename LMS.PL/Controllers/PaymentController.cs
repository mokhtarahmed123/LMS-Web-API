using LMS.API.Bases;
using LMS.Core.Payment.Command.Model;
using LMS.Core.Payment.Query.Model;
using LMS.Data_.Helper;
using LMS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : AppBaseController
    {
        private readonly PaymobSetting _paymobSetting;
        private readonly IPaymobService paymobService;

        public PaymentController(IOptions<PaymobSetting> paymobSetting, IPaymobService paymobService)
        {
            _paymobSetting = paymobSetting.Value;
            this.paymobService = paymobService;
        }

        [EnableRateLimiting("FixedWindowPolicy")]
        [Authorize]
        [HttpPost("create-payment-token")]
        public async Task<IActionResult> CreatePaymentToken([FromQuery] CreatePaymentTokenCommand command)
        {
            var Result = await Mediator.Send(command);
            return Ok(Result);
        }

        [AllowAnonymous]
        [HttpGet("callback")]
        public async Task<IActionResult> CallbackAsync()
        {
            var query = Request.Query;
            string[] fields = new[]
            {
                "amount_cents", "created_at", "currency", "error_occured", "has_parent_transaction",
                "id", "integration_id", "is_3d_secure", "is_auth", "is_capture", "is_refunded",
                "is_standalone_payment", "is_voided", "order", "owner", "pending",
                "source_data.pan", "source_data.sub_type", "source_data.type", "success"
            };

            var concatenated = new StringBuilder();
            foreach (var field in fields)
            {
                if (query.TryGetValue(field, out var value))
                    concatenated.Append(value);
                else
                    return BadRequest($"Missing field: {field}");
            }

            string receivedHmac = query["hmac"];
            string calculatedHmac = paymobService
                .ComputeHmacSHA512(concatenated.ToString(), _paymobSetting.HMAC);

            if (!receivedHmac.Equals(calculatedHmac, StringComparison.OrdinalIgnoreCase))
                return Content(Data_.HtmlGenerator.GenerateSecurityHtml(), "text/html");

            bool.TryParse(query["success"], out bool isSuccess);

            await Mediator.Send(new ProcessPaymentCallbackCommand(
                MerchantOrderId: query["merchant_order_id"],
                IsSuccess: isSuccess
            ));

            return isSuccess
                ? Content(Data_.HtmlGenerator.GenerateSuccessHtml(), "text/html")
                : Content(Data_.HtmlGenerator.GenerateFailedHtml(), "text/html");
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("server-callback")]
        public async Task<IActionResult> ServerCallback([FromBody] JsonElement payload)
        {
            try
            {
                // ✅ IP Whitelisting
                var clientIP = HttpContext.Connection.RemoteIpAddress?.ToString();
                var allowedIPs = _paymobSetting.AllowedIPs;
                if (allowedIPs != null && !allowedIPs.Contains("*") && !allowedIPs.Contains(clientIP))
                    return Unauthorized("IP not allowed");

                string receivedHmac = Request.Query["hmac"];
                string secret = _paymobSetting.HMAC;

                if (!payload.TryGetProperty("obj", out var obj))
                    return BadRequest("Missing 'obj' in payload.");

                string[] fields = new[]
                {
                    "amount_cents", "created_at", "currency", "error_occured", "has_parent_transaction",
                    "id", "integration_id", "is_3d_secure", "is_auth", "is_capture", "is_refunded",
                    "is_standalone_payment", "is_voided", "order.id", "owner", "pending",
                    "source_data.pan", "source_data.sub_type", "source_data.type", "success"
                };

                var concatenated = new StringBuilder();
                foreach (var field in fields)
                {
                    string[] parts = field.Split('.');
                    JsonElement current = obj;
                    bool found = true;

                    foreach (var part in parts)
                    {
                        if (current.ValueKind == JsonValueKind.Object &&
                            current.TryGetProperty(part, out var next))
                            current = next;
                        else
                        {
                            found = false;
                            break;
                        }
                    }

                    if (!found || current.ValueKind == JsonValueKind.Null)
                        concatenated.Append("");
                    else if (current.ValueKind == JsonValueKind.True ||
                             current.ValueKind == JsonValueKind.False)
                        concatenated.Append(current.GetBoolean() ? "true" : "false");
                    else
                        concatenated.Append(current.ToString());
                }

                string calculatedHmac = paymobService
                    .ComputeHmacSHA512(concatenated.ToString(), secret);

                if (!receivedHmac.Equals(calculatedHmac, StringComparison.OrdinalIgnoreCase))
                    return Unauthorized("Invalid HMAC");

                string merchantOrderId = null;
                if (obj.TryGetProperty("order", out var order) &&
                    order.TryGetProperty("merchant_order_id", out var merchantOrderIdElement) &&
                    merchantOrderIdElement.ValueKind != JsonValueKind.Null)
                    merchantOrderId = merchantOrderIdElement.ToString();

                if (string.IsNullOrEmpty(merchantOrderId))
                    return BadRequest("Missing merchant_order_id.");

                bool isSuccess = obj.TryGetProperty("success", out var successElement)
                    && successElement.GetBoolean();

                await Mediator.Send(new ProcessPaymentCallbackCommand(
                    MerchantOrderId: merchantOrderId,
                    IsSuccess: isSuccess
                ));

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("my-payments-history")]
        public async Task<IActionResult> GetMyPaymentsHistory()
        {
            var Result = await Mediator.Send(new GetMyPaymentsHistory());
            return Ok(Result);
        }
    }
}