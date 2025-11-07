using RateLimiter.Application.Cache.Interfaces;
using RateLimiter.Application.RateLimiter.Models;
using StackExchange.Redis;

namespace RateLimiter.Infrastructure.Cache;

internal class DistributedCacheRepository(IConnectionMultiplexer connection) : IDistributedCacheRepository
{
    private const double MinScore = 0;
    private const long MinItemIndex = 0;
    private const long MaxItemIndex = -1; // Represents the last item

    public async Task<GetUserLimitsResponse> GetUserLimitsAsync(GetUserLimitsRequest request)
    {
        var currentTimeInSeconds = request.CurrentTime.ToUnixTimeSeconds();
        var maxScore = request.CurrentTime.Add(-request.UnitsTimeSpan).ToUnixTimeSeconds();

        var redis = connection.GetDatabase();

        var transaction = redis.CreateTransaction();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        transaction.SortedSetRemoveRangeByScoreAsync(request.UserId, MinScore, maxScore, Exclude.None);
        var usersRequestsTasks = transaction.SortedSetRangeByRankAsync(request.UserId, MinItemIndex, MaxItemIndex);

        transaction.SortedSetAddAsync(
            request.UserId,
            new RedisValue(request.CurrentTime.ToString()),
            currentTimeInSeconds);

        transaction.KeyExpireAsync(request.UserId, request.UnitsTimeSpan);

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        await transaction.ExecuteAsync();

        // This call doesn't block anything since result is already represent
        var userRequests = usersRequestsTasks.Result;

        var userRequestsCount = userRequests.Length;
        var leastRequestTime = userRequests.FirstOrDefault();

        return new()
        {
            Count = userRequestsCount,
            LeastRequestTime = leastRequestTime.HasValue ? DateTimeOffset.Parse(leastRequestTime!) : request.CurrentTime,
        };
    }
}
