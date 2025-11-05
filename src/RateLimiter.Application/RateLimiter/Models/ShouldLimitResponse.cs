namespace RateLimiter.Application.RateLimiter.Models;

public class ShouldLimitResponse
{
    public bool Reject { get; private set; }

    public double WaitingSeconds { get; set; }

    public static ShouldLimitResponse NoLimit => new()
    {
        Reject = false,
    };

    public static ShouldLimitResponse CreateLimit(double waitInSeconds) => new()
    {
        Reject = true,
        WaitingSeconds = waitInSeconds
    };
}
