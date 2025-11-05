using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace RateLimiter.Api.Extensions;

internal static class HttpContextExtensions
{
    public static string GetUserId(this HttpContext context) => context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

    public static (string Domain, string Action) GetDomainAndAction(this HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (controllerActionDescriptor != null)
            {
                var controllerName = controllerActionDescriptor.ControllerName;
                var actionName = controllerActionDescriptor.ActionName;

                return (controllerName, actionName);
            }
        }

        return (string.Empty, string.Empty);
    }
}
