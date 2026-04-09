using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class InstructorProfilesRepository : GenericRepositoryAsync<InstructorProfiles>, IInstructorProfilesRepository
    {
        public DbSet<InstructorProfiles> InstructorProfiles { get; set; }
        public InstructorProfilesRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            InstructorProfiles = appDbContext.Set<InstructorProfiles>();
        }

        public async Task<List<InstructorProfiles>> GetAllInstructorProfiles()
        {
            return await InstructorProfiles.Include(i => i.User).ToListAsync();
        }

        public async Task<InstructorProfiles> GetById(int id)
        {
            return await InstructorProfiles.Include(i => i.User).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<InstructorProfiles> GetInstructorProfilesByUserId(string userId)
        {
            return await InstructorProfiles.Include(i => i.User).FirstOrDefaultAsync(i => i.UserId == userId);
        }

        public Task<List<InstructorProfiles>> GetAllInstructorProfilesByFilter(StatusOfInstructorProfileEnum? status)
        {
            return InstructorProfiles
                .Include(i => i.User)
                .Where(i => status == null || i.StatusOfInstructorProfile == status)
                .ToListAsync();
        }

        public async Task<InstructorProfiles> GetMyRequest(string userId)
        {
            return await InstructorProfiles.Include(i => i.User).FirstOrDefaultAsync(i => i.UserId == userId);
        }
    }
}
