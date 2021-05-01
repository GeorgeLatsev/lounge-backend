using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserStatus = Lounge.Services.Notifications.API.Notifications.Methods.UserStatusUpdatedMethod.UserStatus;

namespace Lounge.Services.Notifications.API.Notifications
{
    public class UserConnectionService : IUserConnectionService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly IPublisherService _publisherService;

        public UserConnectionService(
            ConnectionMultiplexer redis,
            IPublisherService publisherService)
        {
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
            _database = redis.GetDatabase();
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        public async Task AddConnectionAsync(string userId, string connectionId)
        {
            await _database.StringSetAsync(GetConnectionKey(connectionId), userId);

            await _database.SetAddAsync(GetUserKey(userId), connectionId);

            var connectionsCount = await _database.SetLengthAsync(GetUserKey(userId));

            if (connectionsCount == 1)
            {
                var subscription = new UserStatusSubscription(userId);
                var method = UserStatusUpdatedMethod.WithArgs(userId, UserStatus.Online);

                await _publisherService.PublishAsync(subscription, method);
            }
        }

        public async Task<ICollection<string>> GetConnectionsAsync(string userId)
        {
            var connections = await _database.SetMembersAsync(GetUserKey(userId));

            return connections
                .Select(r => (string)r)
                .ToArray();
        }

        public async Task<bool> HasConnectionsAsync(string userId)
        {
            var connectionsCount = await _database.SetLengthAsync(GetUserKey(userId));

            return connectionsCount > 0;
        }

        public async Task RemoveConnectionAsync(string userId, string connectionId)
        {
            await _database.KeyDeleteAsync(GetConnectionKey(connectionId));

            await _database.SetRemoveAsync(GetUserKey(userId), connectionId);

            if (!await HasConnectionsAsync(userId))
            {
                var subscription = new UserStatusSubscription(userId);
                var method = UserStatusUpdatedMethod.WithArgs(userId, UserStatus.Offline);

                await _publisherService.PublishAsync(subscription, method);
            }
        }

        public async Task<bool> VerifyConnectionAsync(string userId, string connectionId)
        {
            var connectionUserId = await _database.StringGetAsync(GetConnectionKey(connectionId));

            if (connectionUserId.HasValue)
            {
                return connectionUserId == userId;
            }

            return false;
        }

        private static RedisKey GetConnectionKey(string connectionId) => $":c:{connectionId}";
        private static RedisKey GetUserKey(string userId) => $":u:{userId}";
    }
}
