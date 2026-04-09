using LMS.Data_.Entities;
using LMS.Infrastructure.BackgroundJobs.Subscription;
using LMS.Service;
using LMS.Service.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;
using LMS.Service.BackgroundJobs.Coupon;
using LMS.Service.BackgroundJobs.Courses;
using LMS.Service.BackgroundJobs.InstructorProfile;
using LMS.Service.BackgroundJobs.Quiz;
using LMS.Service.EmailServices;
using LMS.Service.implementation;
using LMS.Service.implementation.Quizzesimplementation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace LMS
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection ServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ICoursesService, CoursesService>();
            services.AddScoped<IInstructorProfilesService, InstructorProfilesService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserCoursesService, UserCoursesService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ILessonsService, LessonsService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<ISubscriptionsService, SubscriptionsService>();
            services.AddScoped<ICouponsService, CouponsService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPaymobService, PaymobService>();
            services.AddScoped<ILessonFilesService, LessonFilesService>();
            services.AddScoped<IUserCouponService, UserCouponService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IQuizQuestionService, QuizQuestionService>();
            services.AddScoped<IQuestionOptionsService, QuestionOptionsService>();
            services.AddScoped<IQuizSubmissionsService, QuizSubmissionsService>();
            // في Infrastructure ServiceExtensions.cs
            services.AddScoped<ICouponJobService, CouponJobService>();
            services.AddScoped<ISubscriptionJobService, SubscriptionJobService>();
            services.AddScoped<IQuizJobService, QuizJobService>();
            services.AddScoped<IInstructorJobService, InstructorJobService>();

            services.AddScoped<ICourseJobService, CourseJobService>();
            services.AddSingleton<ConcurrentDictionary<string, RefreshToken>>();

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 200_000_000;
            });

            return services;
        }
    }
}
