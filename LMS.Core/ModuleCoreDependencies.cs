using FluentValidation;
using LMS.Core.Behavior;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.RateLimiting;

namespace LMS
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(ModuleCoreDependencies).Assembly));

            services.AddAutoMapper(cfg =>
                cfg.AddMaps(typeof(ModuleCoreDependencies).Assembly));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            #region RateLimiter
            services.AddRateLimiter(options =>
            {


                #region FixedWindowLimiter
                options.AddFixedWindowLimiter(policyName: "FixedWindowPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 5;
                    limiterOptions.Window = TimeSpan.FromSeconds(10);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 10;
                });
                #endregion

                #region SlidingWindowLimiter
                options.AddSlidingWindowLimiter(policyName: "SlidingWindowPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 4;
                    limiterOptions.Window = TimeSpan.FromSeconds(10);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 3;
                    limiterOptions.SegmentsPerWindow = 3;
                });
                #endregion

                #region TokenBucketLimiter
                options.AddTokenBucketLimiter(policyName: "TokenBucketPolicy", limiterOptions =>
                {
                    limiterOptions.TokenLimit = 10;
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 5;
                    limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
                    limiterOptions.TokensPerPeriod = 2;
                });
                #endregion

                #region ConcurrencyLimiter
                options.AddConcurrencyLimiter(policyName: "ConcurrencyPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 4;
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 4;
                });
                #endregion


                options.RejectionStatusCode = 429;

                #region GlobalLimiter
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name
                                      ?? httpContext.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 10,
                            QueueLimit = 0,
                            Window = TimeSpan.FromMinutes(1)
                        }));
                #endregion
            });
            #endregion
            return services;
        }
    }
}
