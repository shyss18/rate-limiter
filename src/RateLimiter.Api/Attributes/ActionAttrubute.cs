namespace RateLimiter.Api.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]

public class ActionAttribute(string action) : Attribute
{
    public string Action { get; } = action;
}
