using LMS.Data_.Entities;
using LMS.Data_.Enum;

namespace LMS.Infrastructure.Abstract
{
    public interface IInstructorProfilesRepository : IGenericRepositoryAsync<InstructorProfiles>
    {
        Task<List<InstructorProfiles>> GetAllInstructorProfiles();
        Task<InstructorProfiles> GetById(int id);
        public Task<InstructorProfiles> GetInstructorProfilesByUserId(string userId);
        public Task<List<InstructorProfiles>> GetAllInstructorProfilesByFilter(StatusOfInstructorProfileEnum? status);
        Task<InstructorProfiles> GetMyRequest(string userId);
    }
}
