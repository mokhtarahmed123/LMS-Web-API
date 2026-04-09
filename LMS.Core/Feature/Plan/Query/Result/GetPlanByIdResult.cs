namespace LMS.Core.Feature.Plan.Query.Result
{
    public class GetPlanByIdResult
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationInMonth { get; set; }
        public string Currency { get; set; }

    }
}
