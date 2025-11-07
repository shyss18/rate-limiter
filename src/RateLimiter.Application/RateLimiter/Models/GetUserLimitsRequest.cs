namespace RateLimiter.Application.RateLimiter.Models;

public class GetUserLimitsRequest
{
    public required string UserId { get; set; }

    public DateTimeOffset CurrentTime { get; set; }

    public TimeSpan UnitsTimeSpan { get; set; }
}
