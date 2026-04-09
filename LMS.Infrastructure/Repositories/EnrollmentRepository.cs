using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class EnrollmentRepository : GenericRepositoryAsync<Enrollment>, IEnrollmentRepository
    {
        private readonly DbSet<Enrollment> Enrollment;
        public EnrollmentRepository(AppDbContext dbContext) : base(dbContext)
        {
            Enrollment = dbContext.Set<Enrollment>();

        }
    }
}
