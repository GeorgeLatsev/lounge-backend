using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using System;
using System.Threading.Tasks;
using UserStatus = Lounge.Services.Notifications.API.Notifications.Methods.UserStatusUpdatedMethod.UserStatus;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling
{
    public class ConnectionUpdatedIntegrationEventHandler : IIntegrationEventHandler<ConnectionUpdatedIntegrationEvent>
    {
        private readonly IPublisherService _publisherService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IUserConnectionService _connectionService;

        public ConnectionUpdatedIntegrationEventHandler(
            IPublisherService publisherService,
            ISubscriptionService subscriptionService,
            IUserConnectionService connectionService)
        {
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _connectionService = connectionService ?? throw new ArgumentNullException(nameof(connectionService));
        }

        public async Task Handle(ConnectionUpdatedIntegrationEvent @event)
        {
            var connectionSubscription = new ConnectionSubscription(@event.UserId, @event.OtherId);
            var connectionMethod = ConnectionUpdatedMethod.WithArgs(@event.OtherId, @event.Notes, (int)@event.Relationship);

            await _publisherService.PublishAsync(connectionSubscription, connectionMethod);

            var statusSubscription = new UserStatusSubscription(@event.OtherId);

            var connections = await _connectionService.GetConnectionsAsync(@event.UserId);
            foreach (var connection in connections)
            {
                if (@event.Relationship == ConnectionUpdatedIntegrationEvent.RelationshipEnum.Friend)
                {
                    await _subscriptionService.SubscribeAsync(connection, statusSubscription);

                    var isOnline = await _connectionService.HasConnectionsAsync(@event.OtherId);
                    var statusMethod = UserStatusUpdatedMethod.WithArgs(@event.OtherId, isOnline ? UserStatus.Online : UserStatus.Offline);

                    await _publisherService.PublishAsync(connection, statusMethod);
                }
                else
                {
                    await _subscriptionService.UnsubscribeAsync(connection, statusSubscription);

                    var statusMethod = UserStatusUpdatedMethod.WithArgs(@event.OtherId, UserStatus.Unknown);

                    await _publisherService.PublishAsync(connection, statusMethod);
                }
            }
        }
    }
}
