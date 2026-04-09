namespace LMS.Core.Feature.Authorization.Query.Result
{
    public class GetAllRolesResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int UsersCount { get; set; }
        public List<GetAllUsersQueryResultWithRole> Users { get; set; } = new List<GetAllUsersQueryResultWithRole>();

    }
    public class GetAllUsersQueryResultWithRole
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

    }
}