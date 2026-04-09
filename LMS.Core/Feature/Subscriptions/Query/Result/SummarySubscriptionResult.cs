namespace LMS.Core.Feature.Subscriptions.Query.Result
{
    public class SummarySubscriptionResult
    {
        public int TotalSubscriptions { get; set; }
        public int SuccessSubscriptions { get; set; }
        public int PendingSubscriptions { get; set; }
        public int FailedSubscriptions { get; set; }
        public int RefundedSubscriptions { get; set; }
        public int NewSubscriptionsThisMonth { get; set; }


        public decimal TotalRevenue { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public decimal AverageSubscriptionValue { get; set; }
        public decimal TotalDiscountsGiven { get; set; }


        public int SubscriptionsExpiringThisWeek { get; set; }
        public int SubscriptionsExpiringThisMonth { get; set; }

        public List<PlanSummary> PlansSummary { get; set; }


    }
    public class PlanSummary
    {
        public string PlanName { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }
}
