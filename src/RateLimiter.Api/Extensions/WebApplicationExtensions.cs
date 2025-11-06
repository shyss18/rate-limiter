using Microsoft.AspNetCore.Builder;
using RateLimiter.Api.Middlewares;

namespace RateLimiter.Api.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseRateLimiterMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<RateLimitMiddleware>();

        return app;
    }
}
