namespace LMS.Core.Feature.Subscriptions.Query.Result
{

    public class GetSubscriptionByUserIdResponse
    {
        public int CountOfSubscriptions { get; set; }
        public List<GetSubscriptionByUserIdResult> GetSubscriptions { get; set; }

    }
    public class GetSubscriptionByUserIdResult
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; }
        public string NameofPlan { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }

    }
}
