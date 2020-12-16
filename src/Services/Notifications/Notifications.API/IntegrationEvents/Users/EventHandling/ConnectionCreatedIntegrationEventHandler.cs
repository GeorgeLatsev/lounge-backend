using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using System;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling
{
    public class ConnectionCreatedIntegrationEventHandler : IIntegrationEventHandler<ConnectionCreatedIntegrationEvent>
    {
        private readonly IUserConnectionService _userConnectionService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPublisherService _publisherService;

        public ConnectionCreatedIntegrationEventHandler(
            IUserConnectionService userConnectionService,
            ISubscriptionService subscriptionService,
            IPublisherService publisherService)
        {
            _userConnectionService = userConnectionService ?? throw new ArgumentNullException(nameof(userConnectionService));
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        public async Task Handle(ConnectionCreatedIntegrationEvent @event)
        {
            var userSubscription = new UserSubscription(@event.OtherId);
            var connectionSubscription = new ConnectionSubscription(@event.UserId, @event.OtherId);

            var connectionIds = await _userConnectionService.GetConnectionsAsync(@event.UserId);
            foreach (var connectionId in connectionIds)
            {
                await _subscriptionService.SubscribeAsync(connectionId, userSubscription);
                await _subscriptionService.SubscribeAsync(connectionId, connectionSubscription);
            }

            var method = ConnectionCreatedMethod.WithArgs(@event.OtherId, @event.Name, @event.Tag);

            await _publisherService.PublishAsync(connectionSubscription, method);
        }
    }
}
