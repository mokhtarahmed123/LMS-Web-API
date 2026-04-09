namespace LMS.Core.Feature.Subscriptions.Query.Result
{
    public class GetAllSubscriptionsResponse
    {
        public int CountOfActiveSubscriptions { get; set; }
        public int CountOfUnActiveSubscriptions { get; set; }
        public List<GetAllSubscriptionsResult> Subscriptions { get; set; }
    }
    public class GetAllSubscriptionsResult
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
