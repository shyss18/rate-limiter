using RateLimiter.Application.Cache.Interfaces;
using RateLimiter.Application.RateLimiter.Models;
using StackExchange.Redis;

namespace RateLimiter.Infrastructure.Cache;

internal class DistributedCacheRepository(IConnectionMultiplexer connection) : IDistributedCacheRepository
{
    public async Task<GetUserLimitsResponse> GetUserLimitsAsync(GetUserLimitsRequest request)
    {
        var redis = connection.GetDatabase();

        var transaction = redis.CreateTransaction();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        transaction.SortedSetRemoveRangeByScoreAsync(request.UserId, request.LowBoundTime.TotalSeconds, request.HighBoundTime.TotalSeconds);
        var usersRequests = transaction.SortedSetRangeByRankAsync(request.UserId, 0, -1);

        transaction.SortedSetAddAsync(
            request.UserId,
            new RedisValue(request.CurrentTime.ToString()),
            request.CurrentTime.TotalSeconds);

        transaction.KeyExpireAsync(request.UserId, request.TimeToLive);

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        await transaction.ExecuteAsync();

        var userRequestsCount = usersRequests.Result.Length;
        var lastRequestTime = usersRequests.Result.FirstOrDefault();

        return new()
        {
            Count = userRequestsCount,
            LeastRequestTime = lastRequestTime.HasValue ? TimeSpan.Parse(lastRequestTime!) : DateTime.Now.TimeOfDay,
        };
    }
}
