using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RateLimiter.Api.Extensions;
using RateLimiter.Application.RateLimiter.Interfaces;
using RateLimiter.Application.RateLimiter.Models;
using System.Net;

namespace RateLimiter.Api.Middlewares;

internal sealed class RateLimitMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IRateLimiter rateLimiter,
        ILogger<RateLimitMiddleware> logger)
    {
        var userId = context.GetUserId();

        logger.LogInformation("Detected userId = {userId}", userId);

        var (Domain, Action) = context.GetDomainAndAction();

        logger.LogInformation("Detected domain = {userId} and action = {action}", Domain, Action);

        var response = await rateLimiter.ShouldLimitRequestAsync(new ShouldLimitRequest(userId, Domain, Action));
        if (response.Reject)
        {
            logger.LogInformation("Reject request with statusCode {statusCode}", HttpStatusCode.TooManyRequests);

            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers.RetryAfter = response.WaitingSeconds.ToString();

            await context.Response.WriteAsync("Too many requests. Please try again later.");

            return;
        }

        await next(context);
    }
}
