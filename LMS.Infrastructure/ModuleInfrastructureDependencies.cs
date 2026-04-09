using LMS.Data_;
using LMS.Data_.Helper;
using LMS.Infrastructure;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Caching.Redis;
using LMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
namespace LMS
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection InfrastructureDependencies(this IServiceCollection services, IConfiguration Configuration)
        {



            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<ICoursesRepository, CoursesRepository>();
            services.AddScoped<IInstructorProfilesRepository, InstructorProfilesRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserCoursesRepository, UserCoursesRepository>();
            services.AddScoped<ILessonsRepository, LessonsRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
            services.AddScoped<ICouponsRepository, CouponsRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPaymobRepository, PaymobRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<ILessonFilesRepository, LessonFilesRepository>();
            services.AddScoped<IUserCouponRepository, UserCouponRepository>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IQuizQuestionsRepository, QuizQuestionsRepository>();
            services.AddScoped<IQuestionOptionsRepository, QuestionOptionsRepository>();
            services.AddScoped<IQuizSubmissionsRepository, QuizSubmissionsRepository>();
            services.AddScoped<ISubmissionAnswersRepository, SubmissionAnswersRepository>();



            services.AddHttpClient();
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LMS Project",
                    Version = "v1"
                });

                c.EnableAnnotations();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token here without adding 'Bearer'. Swagger adds it automatically."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });
            #endregion

            #region Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                  .AddJwtBearer(options =>
                  {
                      var secretKey = Configuration["JWT:SecretKey"];
                      if (string.IsNullOrEmpty(secretKey))
                          throw new ArgumentNullException("JWT:SecretKey", "JWT SecretKey is missing in appsettings.json");

                      options.SaveToken = true;
                      options.RequireHttpsMetadata = false;
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidIssuer = Configuration["JWT:IssuerIP"],

                          ValidateAudience = true,
                          ValidAudience = Configuration["JWT:AudienceIP"],

                          ValidateIssuerSigningKey = true,
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                          RoleClaimType = "roleName",
                          ValidateLifetime = true,
                          ClockSkew = TimeSpan.Zero
                      };
                  });

            #endregion


            services.Configure<EmailSettings>(Configuration.GetSection("Email"));

            services.Configure<PaymobSetting>(Configuration.GetSection("Paymob"));

            return services;
        }
    }
}
