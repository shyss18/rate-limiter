namespace RateLimiter.Application.RateLimiter.Models;

public class GetUserLimitsRequest
{
    public required string UserId { get; set; }

    public TimeSpan HighBoundTime { get; set; }

    public TimeSpan LowBoundTime { get; set; }

    public TimeSpan CurrentTime { get; set; }

    public TimeSpan TimeToLive { get; set; }
}
