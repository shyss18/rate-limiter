namespace RateLimiter.Infrastructure.Options;

public class RateLimiterOptions
{
    public string RedisConnection { get; set; } = null!;

    public bool UseConfigurationOptions { get; set; }
}
