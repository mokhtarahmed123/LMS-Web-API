using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using LMS.Data_.Entities;
using LMS.Data_.Entities.Quiz;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<Users, Role, string>
    {
        #region Tables
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Coupons> Coupons { get; set; }

        public DbSet<Courses> Courses { get; set; }
        public DbSet<InstructorProfiles> InstructorProfiles { get; set; }
        public DbSet<Lessons> Lessons { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<UserCoupons> UserCoupons { get; set; }
        public DbSet<UserCourses> UserCourses { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<LessonFiles> LessonFiles { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<Quizzes> Quizzes { get; set; }
        public DbSet<QuizQuestions> QuizQuestions { get; set; }
        public DbSet<QuestionOptions> QuestionOptions { get; set; }
        public DbSet<QuizSubmissions> QuizSubmissions { get; set; }
        public DbSet<SubmissionAnswers> SubmissionAnswers { get; set; }











        #endregion

        private readonly IEncryptionProvider encryptionProvider;
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            encryptionProvider = new GenerateEncryptionProvider("paoiebfec64f99d943d983ed99fabcd1");
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseEncryption(encryptionProvider);

            builder.Entity<Payments>()
            .HasIndex(p => p.TransactionId)
             .IsUnique();

            builder.Entity<Coupons>()
                .HasIndex(c => c.Code)
            .IsUnique();

            builder.Entity<Lessons>()
           .HasIndex(l => new { l.CourseId, l.OrderNumber })
           .IsUnique();

            builder.Entity<UserCourses>()
                .HasKey(o => new { o.CourseId, o.UserId });

            builder.Entity<UserCoupons>()
                   .HasKey(o => new { o.CouponId, o.UserId });


            builder.Entity<SubmissionAnswers>()
                   .HasKey(o => new { o.SubmissionId, o.QuestionId });




            base.OnModelCreating(builder);




        }
    }
}
