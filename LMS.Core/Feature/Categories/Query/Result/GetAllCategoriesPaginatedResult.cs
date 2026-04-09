namespace LMS.Core.Feature.Categories.Query.Result
{
    public class GetAllCategoriesPaginatedResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public GetAllCategoriesPaginatedResult(int Id, string name, string Description, bool isactive)
        {
            this.Id = Id;
            this.Name = name;
            this.Description = Description;
            this.IsActive = isactive;
        }

    }
}
