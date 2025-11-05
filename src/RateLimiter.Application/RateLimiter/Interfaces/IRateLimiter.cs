using RateLimiter.Application.RateLimiter.Models;

namespace RateLimiter.Application.RateLimiter.Interfaces;

public interface IRateLimiter
{
    Task<ShouldLimitResponse> ShouldLimitRequestAsync(ShouldLimitRequest request);
}
