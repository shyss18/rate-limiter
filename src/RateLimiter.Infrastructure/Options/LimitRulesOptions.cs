using RateLimiter.Domain.Rules.Models;

namespace RateLimiter.Infrastructure.Options;

internal class LimitRulesOptions
{
    public LimiterRule[] Rules { get; set; } = [];
}
