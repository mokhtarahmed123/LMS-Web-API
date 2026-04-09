namespace LMS.Core.Feature.Subscriptions.Query.Result
{
    public class CheckActiveSubscriptionResult
    {
        public bool isActive { get; set; }
        public DateOnly endDate { get; set; }
    }
}
