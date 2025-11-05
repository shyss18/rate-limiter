using RateLimiter.Domain.Rules.Models;

namespace RateLimiter.Application.LimiterRules.Interfaces;

public interface ILimiterRulesRepository
{
    ValueTask<LimiterRule[]> GetLimiterRulesAsync(string domain);
}
