using Microsoft.Extensions.Options;
using RateLimiter.Application.LimiterRules.Interfaces;
using RateLimiter.Domain.Rules.Models;
using RateLimiter.Infrastructure.Options;

namespace RateLimiter.Infrastructure.LimiterRules;

internal class OptionsLimiterRulesRepository(IOptions<LimitRulesOptions> options) : ILimiterRulesRepository
{
    public ValueTask<LimiterRule[]> GetLimiterRulesAsync(string domain)
    {
        var domainRules = options.Value.Rules.Where(x => x.Domain.Equals(domain, StringComparison.OrdinalIgnoreCase)).ToArray();
        return ValueTask.FromResult(domainRules);
    }
}
