using RateLimiter.Domain.Rules.Enums;

namespace RateLimiter.Domain.Rules.Models;

/// <summary>
/// Describe algorithm for limiter
/// </summary>
public class LimitRuleDescription
{
    /// <summary>
    /// The action where rule should be applied
    /// </summary>
    public required string Action { get; set; }

    /// <summary>
    /// Time interval for rule
    /// </summary>
    public required LimitUnit Unit { get; set; }

    /// <summary>
    /// The number of requests which can be processed during <see cref="LimitUnit"/>
    /// </summary>
    public required int RequestsPerUnit { get; set; }

    public TimeSpan GetTimeSpanFromUnits() => Unit switch
    {
        LimitUnit.Millisecond => TimeSpan.FromMilliseconds(1),
        LimitUnit.Second => TimeSpan.FromSeconds(1),
        LimitUnit.Minute => TimeSpan.FromMinutes(1),
        LimitUnit.Hour => TimeSpan.FromHours(1),
        LimitUnit.Day => TimeSpan.FromDays(1),
        LimitUnit.Week => TimeSpan.FromDays(7),
        LimitUnit.Month => TimeSpan.FromDays(30),
        _ => throw new NotImplementedException(),
    };
}
