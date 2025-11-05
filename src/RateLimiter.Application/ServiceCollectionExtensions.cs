using Microsoft.Extensions.DependencyInjection;
using RateLimiter.Application.RateLimiter.Interfaces;

namespace RateLimiter.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRateLimiter, RateLimiter.RateLimiter>();

        return services;
    }
}
