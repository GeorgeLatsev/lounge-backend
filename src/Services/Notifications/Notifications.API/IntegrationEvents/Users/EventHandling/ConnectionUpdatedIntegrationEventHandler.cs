using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using System;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling
{
    public class ConnectionUpdatedIntegrationEventHandler : IIntegrationEventHandler<ConnectionUpdatedIntegrationEvent>
    {
        private readonly IPublisherService _publisherService;
        
        public ConnectionUpdatedIntegrationEventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        public async Task Handle(ConnectionUpdatedIntegrationEvent @event)
        {
            var subscription = new ConnectionSubscription(@event.UserId, @event.OtherId);
            var method = ConnectionUpdatedMethod.WithArgs(@event.OtherId, @event.Notes, (int)@event.Relationship);

            await _publisherService.PublishAsync(subscription, method);
        }
    }
}
