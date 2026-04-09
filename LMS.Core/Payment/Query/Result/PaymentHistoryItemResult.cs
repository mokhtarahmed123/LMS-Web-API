namespace LMS.Core.Payment.Query.Result
{
    public class PaymentHistoryItemResult
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public int SubscriptionId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string PlanName { get; set; }




    }
}
