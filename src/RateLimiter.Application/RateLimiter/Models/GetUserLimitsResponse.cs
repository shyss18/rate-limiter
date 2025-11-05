namespace RateLimiter.Application.RateLimiter.Models;

public class GetUserLimitsResponse
{
    public required int Count { get; set; }

    public required TimeSpan LeastRequestTime { get; set; }
}
