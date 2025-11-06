using Microsoft.AspNetCore.Http;
using RateLimiter.Api.Attributes;

namespace RateLimiter.Api.Extensions;

internal static class HttpContextExtensions
{
    public static string GetUserId(this HttpContext context) => context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

    public static (string Domain, string Action) GetDomainAndAction(this HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var domainAttribute = endpoint.Metadata.GetMetadata<DomainAttribute>();
            var actionAttribute = endpoint.Metadata.GetMetadata<ActionAttribute>();

            if (domainAttribute != null && actionAttribute != null)
            {
                return (domainAttribute.Domain, actionAttribute.Action);
            }
        }

        return (string.Empty, string.Empty);
    }
}
