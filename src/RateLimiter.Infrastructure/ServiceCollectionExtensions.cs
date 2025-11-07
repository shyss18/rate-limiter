using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RateLimiter.Application.Cache.Interfaces;
using RateLimiter.Application.LimiterRules.Interfaces;
using RateLimiter.Infrastructure.Cache;
using RateLimiter.Infrastructure.LimiterRules;
using RateLimiter.Infrastructure.Options;
using StackExchange.Redis;

namespace RateLimiter.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        RateLimiterOptions rateLimiterOptions)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(rateLimiterOptions.RedisConnection));

        services.AddScoped<IDistributedCacheRepository, DistributedCacheRepository>();

        if (rateLimiterOptions.UseConfigurationOptions)
        {
            services.AddScoped<ILimiterRulesRepository, OptionsLimiterRulesRepository>();
            services.Configure<LimitRulesOptions>(configuration.GetSection(nameof(LimitRulesOptions)));
        }

        return services;
    }
}
