using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract;
using Microsoft.AspNetCore.Http;

namespace LMS.Service.implementation
{
    public class InstructorProfilesService : IInstructorProfilesService
    {
        private readonly IInstructorProfilesRepository instructorProfilesRepository;
        private readonly IFileService fileService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public InstructorProfilesService(IInstructorProfilesRepository instructorProfilesRepository, IFileService fileService, IHttpContextAccessor httpContextAccessor)
        {
            this.instructorProfilesRepository = instructorProfilesRepository;
            this.fileService = fileService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<InstructorProfiles> Add(InstructorProfiles instructor, IFormFile File)
        {
            var cotext = httpContextAccessor.HttpContext.Request;
            var baseurl = cotext.Scheme + "://" + cotext.Host;
            var ImageUrl = await fileService.UploadImage("Instructors", File);
            instructor.ProfilePictureUrl = baseurl + ImageUrl;
            var Inst = await instructorProfilesRepository.AddAsync(instructor);
            if (Inst != null)
                return Inst;
            return null;

        }

        public async Task<bool> Delete(int id)
        {
            using var transaction = instructorProfilesRepository.BeginTransaction();
            try
            {
                var Instructor = await instructorProfilesRepository.GetByIdAsync(id);

                if (Instructor == null)
                    return false;

                if (!string.IsNullOrEmpty(Instructor.ProfilePictureUrl))
                {
                    await fileService.DeleteFileAsync(Instructor.ProfilePictureUrl);
                }

                await instructorProfilesRepository.DeleteAsync(Instructor);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<List<InstructorProfiles>> GetAll()
        {
            return await instructorProfilesRepository.GetAllInstructorProfiles();
        }

        public IQueryable<InstructorProfiles> GetAllInstructorsAsQueryable()
        {
            return instructorProfilesRepository.GetTableNoTracking().AsQueryable();
        }

        public async Task<List<InstructorProfiles>> GetAllInstructorsFilter(StatusOfInstructorProfileEnum? status)
        {
            return await instructorProfilesRepository.GetAllInstructorProfilesByFilter(status);
        }

        public async Task<InstructorProfiles> GetById(int id)
        {
            return await instructorProfilesRepository.GetByIdAsync(id);
        }

        public async Task<InstructorProfiles> GetInstructorByUserId(string userId)
        {
            return await instructorProfilesRepository.GetInstructorProfilesByUserId(userId);
        }

        public async Task<InstructorProfiles> GetInstructorProfilesByUserId(string userId)
        {
            return await instructorProfilesRepository.GetInstructorProfilesByUserId(userId);
        }

        public async Task<InstructorProfiles> GetMyRequest(string userId)
        {
            return await instructorProfilesRepository.GetMyRequest(userId);


        }

        public async Task<InstructorProfiles> Update(InstructorProfiles instructor, string? Reason)
        {
            instructor.ReasonOfReject = Reason;
            await instructorProfilesRepository.UpdateAsync(instructor);
            return instructor;

        }

        public async Task<InstructorProfiles> UpdateWithImage(InstructorProfiles instructor, IFormFile file)
        {
            if (file != null)
            {
                var request = httpContextAccessor.HttpContext?.Request
                    ?? throw new InvalidOperationException("No HTTP context");

                var baseUrl = $"{request.Scheme}://{request.Host}";

                var imageUrl = await fileService.UploadImage("Instructors", file);

                instructor.ProfilePictureUrl = baseUrl + imageUrl;
            }

            await instructorProfilesRepository.UpdateAsync(instructor);
            return instructor;

        }
    }
}
