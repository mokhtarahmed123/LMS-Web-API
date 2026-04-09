using Hangfire;
using LMS.Core.HandlerMiddleware;
using LMS.Data_.Entities;
using LMS.Infrastructure.BackgroundJobs.Subscription;
using LMS.Infrastructure.Context;
using LMS.Service.BackgroundJobs.Coupon;
using LMS.Service.BackgroundJobs.Courses;
using LMS.Service.BackgroundJobs.Quiz;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.Json.Serialization;

namespace LMS.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();

            #region Database
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("LMS"))
            );
            // Cache
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });


            #endregion
            #region CORS
            builder.Services.AddCors(options =>
                options.AddPolicy("CorsPolicy", policyBuilder =>
                    policyBuilder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader())
            );
            #endregion

            #region Identity
            builder.Services.AddIdentity<Users, Role>(
                opt =>
               {
                   opt.Password.RequireDigit = true;
                   opt.Password.RequireLowercase = true;
                   opt.Password.RequireUppercase = true;
                   opt.Password.RequiredLength = 8;
                   opt.Password.RequireNonAlphanumeric = false;
                   opt.User.RequireUniqueEmail = true;
                   opt.SignIn.RequireConfirmedEmail = true;
                   opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                   opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

               }

                )

                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            #endregion
            #region Localization
            //builder.Services.AddControllersWithViews();
            builder.Services.AddLocalization(options => options.ResourcesPath = "");
            builder.Services.Configure<RequestLocalizationOptions>(opt =>
            {
                List<CultureInfo> SupportCulture = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar-EG")

                };
                opt.DefaultRequestCulture = new RequestCulture("en-US");
                opt.SupportedCultures = SupportCulture;
                opt.SupportedUICultures = SupportCulture;

            });
            #endregion
            #region Hangfire
            builder.Services.AddHangfire(a => a.UseSqlServerStorage(builder.Configuration.GetConnectionString("LMS")));
            builder.Services.AddHangfireServer();
            #endregion
            #region DI
            builder.Services.InfrastructureDependencies(builder.Configuration).ServiceDependencies()
                .AddCoreDependencies();
            #endregion





            builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(
                            new JsonStringEnumConverter()
                        );
                    });

            var app = builder.Build();



            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();


            app.UseHangfireDashboard("/dashboard");

            #region Background JobService

            // Subscription
            RecurringJob.AddOrUpdate<ISubscriptionJobService>(
             "check-expired-subscriptions",
                job => job.CheckExpiredSubscriptions(),
          Cron.Daily);

            RecurringJob.AddOrUpdate<ISubscriptionJobService>(
             "notify-expiring-subscriptions",
          job => job.NotifyExpiringSubscriptions(),
               Cron.Daily);

            // Coupon

            RecurringJob.AddOrUpdate<ICouponJobService>(
                "check-expired-coupon", job => job.CheckExpiredCoupons(), Cron.Daily
                );

            // Quiz

            RecurringJob.AddOrUpdate<IQuizJobService>(
                "check-expired-quiz", job => job.CheckExpiredQuizzes(), Cron.Daily
                );


            // Course
            RecurringJob.AddOrUpdate<ICourseJobService>(
             "auto-reject-pending-courses", job => job.AutoRejectLongPendingCourses(), Cron.Daily
             );

            // Instructor Profile



            #endregion 

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<TokenBlacklistMiddleware>();
            #region Localization Middleware
            var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            #endregion
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            app.UseRateLimiter();

            #region paymobAllowedIPs
            var paymobAllowedIPs = app.Configuration
                .GetSection("Paymob:AllowedIPs")
                .Get<string[]>();

            app.UseWhen(
                context => context.Request.Path.StartsWithSegments("/api/Payment/callback") ||
                           context.Request.Path.StartsWithSegments("/api/Payment/server-callback"),
                builder => builder.Use(async (context, next) =>
                {

                    if (paymobAllowedIPs == null || paymobAllowedIPs.Contains("*"))
                    {
                        await next();
                        return;
                    }

                    var clientIP = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                   ?? context.Connection.RemoteIpAddress?.ToString();

                    Console.WriteLine($"==> Client IP: {clientIP}");

                    if (!paymobAllowedIPs.Contains(clientIP))
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Forbidden");
                        return;
                    }

                    await next();
                }));
            #endregion      

            app.MapControllers();

            app.Run();
        }
    }
}
