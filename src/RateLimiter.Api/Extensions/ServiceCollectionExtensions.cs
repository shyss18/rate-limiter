using Microsoft.Extensions.DependencyInjection;
using RateLimiter.Infrastructure;
using RateLimiter.Infrastructure.Options;

namespace RateLimiter.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRateLimiter(
        this IServiceCollection services,
        Action<RateLimiterOptions> action)
    {
        var options = new RateLimiterOptions();

        action(options);

        services.AddInfrastructure(options);

        return services;
    }
}
