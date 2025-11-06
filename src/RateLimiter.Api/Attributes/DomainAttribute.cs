namespace RateLimiter.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]

public class DomainAttribute(string domain) : Attribute
{
    public string Domain { get; } = domain;
}
