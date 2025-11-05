namespace RateLimiter.Domain.Rules.Enums;

/// <summary>
/// Time interval for applying rule
/// </summary>
public enum LimitUnit
{
    Millisecond = 0,

    Second = 1,

    Minute = 2,

    Hour = 3,

    Day = 4,

    Week = 5,

    Month = 6
}
