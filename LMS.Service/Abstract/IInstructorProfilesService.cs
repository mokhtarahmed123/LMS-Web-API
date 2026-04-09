using LMS.Data_.Entities;
using LMS.Data_.Enum;
using Microsoft.AspNetCore.Http;

namespace LMS.Service.Abstract
{
    public interface IInstructorProfilesService
    {
        public Task<InstructorProfiles> Add(InstructorProfiles instructor, IFormFile file);
        public Task<List<InstructorProfiles>> GetAll();
        public Task<InstructorProfiles> GetById(int id);
        public Task<InstructorProfiles> Update(InstructorProfiles instructor, string? Reason);
        public Task<InstructorProfiles> UpdateWithImage(InstructorProfiles instructor, IFormFile file);
        Task<InstructorProfiles> GetInstructorProfilesByUserId(string userId);
        Task<InstructorProfiles> GetInstructorByUserId(string userId);

        IQueryable<InstructorProfiles> GetAllInstructorsAsQueryable();
        Task<List<InstructorProfiles>> GetAllInstructorsFilter(StatusOfInstructorProfileEnum? status);

        Task<InstructorProfiles> GetMyRequest(string userId);


        public Task<bool> Delete(int id);
    }
}
