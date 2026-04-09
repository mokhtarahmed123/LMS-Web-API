namespace LMS.Core.Feature.Subscriptions.Query.Result
{
    public class GetMySubscriptionResult
    {
        public string Email { get; set; }
        public int Number { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; }
        public string SubscriptionStatus { get; set; }
        public int Discount { get; set; }

        public string NameOfPlan { get; set; }
    }
}
