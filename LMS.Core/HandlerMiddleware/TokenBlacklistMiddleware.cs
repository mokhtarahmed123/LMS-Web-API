using LMS.Infrastructure.Caching.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class TokenBlacklistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory scopeFactory;

    public TokenBlacklistMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        this.scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            using var scope = scopeFactory.CreateScope();
            var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

            var isBlacklisted = await cacheService.GetDataAsync<string>($"blacklist:{token}");
            if (isBlacklisted != null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { message = "Token has been revoked" });
                return;
            }
        }

        await _next(context);
    }
}