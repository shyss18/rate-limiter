namespace RateLimiter.Domain.Rules.Models;

/// <summary>
/// Describing how to apply limiter
/// </summary>
public class LimiterRule
{
    /// <summary>
    /// The area where rule should work
    /// </summary>
    public required string Domain { get; set; }

    /// <summary>
    /// Describe algorithm for limiter
    /// </summary>
    public required LimitRuleDescription Description { get; set; }
}
