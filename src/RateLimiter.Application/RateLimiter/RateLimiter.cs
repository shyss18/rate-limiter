using Microsoft.Extensions.Logging;
using RateLimiter.Application.Cache.Interfaces;
using RateLimiter.Application.LimiterRules.Interfaces;
using RateLimiter.Application.RateLimiter.Interfaces;
using RateLimiter.Application.RateLimiter.Models;

namespace RateLimiter.Application.RateLimiter;

internal class RateLimiter(
    IDistributedCacheRepository cacheRepository,
    ILimiterRulesRepository limiterRulesRepository,
    ILogger<RateLimiter> logger) : IRateLimiter
{
    public async Task<ShouldLimitResponse> ShouldLimitRequestAsync(ShouldLimitRequest request)
    {
        var limiterRules = await limiterRulesRepository.GetLimiterRulesAsync(request.Domain);
        if (limiterRules.Length == 0)
        {
            logger.LogInformation("Did not find any rules for domain = {domain}", request.Domain);

            return ShouldLimitResponse.NoLimit;
        }

        logger.LogInformation("Found {rulesCount} rules for domain = {domain}", limiterRules.Length, request.Domain);

        var actionRule = limiterRules.FirstOrDefault(x => x.Description.Action.Equals(request.Action, StringComparison.OrdinalIgnoreCase));
        if (actionRule == null)
        {
            logger.LogInformation("Did not find any rules for action = {action}", request.Action);

            return ShouldLimitResponse.NoLimit;
        }

        logger.LogInformation("Found action rule for domain = {domain}, action = {action}, unit = {unit}, requestsPerUnit = {requestsPerUnit}",
            request.Domain,
            request.Action,
            actionRule.Description.Unit,
            actionRule.Description.RequestsPerUnit);

        var currentTime = DateTime.Now.TimeOfDay;
        var unitsTimeSpan = actionRule.Description.GetTimeSpanFromUnits();

        var response = await cacheRepository.GetUserLimitsAsync(new GetUserLimitsRequest
        {
            UserId = request.UserId,
            CurrentTime = currentTime,
            TimeToLive = currentTime.Add(unitsTimeSpan),
            LowBoundTime = currentTime.Add(-unitsTimeSpan),
            HighBoundTime = currentTime,
        });

        var shouldLimit = response.Count > actionRule.Description.RequestsPerUnit;
        var waitTimeSpan = response.LeastRequestTime.Add(unitsTimeSpan) - currentTime;

        logger.LogInformation("Received response user limits, count = {userLimitsCount}, leastRequestTime = {LeastRequestTime}, shouldLimit = {shouldLimit}, waitInSeconds = {waitInSeconds}",
            response.Count,
            response.LeastRequestTime,
            shouldLimit,
            waitTimeSpan.TotalSeconds);

        return shouldLimit
            ? ShouldLimitResponse.CreateLimit(waitTimeSpan.TotalSeconds)
            : ShouldLimitResponse.NoLimit;
    }
}
