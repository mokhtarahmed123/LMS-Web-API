namespace LMS.Core.Feature.Plan.Query.Result
{
    public class GetAllPlanResultForUsers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int DurationInMonth { get; set; }
    }
}
