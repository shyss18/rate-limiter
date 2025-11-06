using Microsoft.Extensions.DependencyInjection;
using RateLimiter.Application;
using RateLimiter.Infrastructure;
using RateLimiter.Infrastructure.Options;

namespace RateLimiter.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRateLimiter(
        this IServiceCollection services,
        Action<RateLimiterOptions> action)
    {
        var options = new RateLimiterOptions();

        action(options);

        services.AddApplication();
        services.AddInfrastructure(options);

        return services;
    }
}
