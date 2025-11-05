using RateLimiter.Application.RateLimiter.Models;

namespace RateLimiter.Application.Cache.Interfaces;

public interface IDistributedCacheRepository
{
    Task<GetUserLimitsResponse> GetUserLimitsAsync(GetUserLimitsRequest request);
}
