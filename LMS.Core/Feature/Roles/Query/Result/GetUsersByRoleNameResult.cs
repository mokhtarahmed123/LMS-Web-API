namespace LMS.Core.Feature.Authorization.Query.Result
{
    public class GetUsersByRoleNameResult
    {
        public string RoleName { get; set; }
        public List<GetAllUsersQueryResultWithRole> Users { get; set; }
        public int CountOfUsers { get; set; }
    }
}
