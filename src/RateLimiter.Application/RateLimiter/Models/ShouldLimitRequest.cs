namespace RateLimiter.Application.RateLimiter.Models;

public class ShouldLimitRequest(string userId, string domain, string action)
{
    public string UserId => userId;

    public string Domain => domain;

    public string Action => action;
}
