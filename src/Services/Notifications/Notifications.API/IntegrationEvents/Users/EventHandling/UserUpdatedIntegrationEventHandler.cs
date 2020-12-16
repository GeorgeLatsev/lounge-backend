using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using System;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling
{
    public class UserUpdatedIntegrationEventHandler : IIntegrationEventHandler<UserUpdatedIntegrationEvent>
    {
        private readonly IPublisherService _publisherService;

        public UserUpdatedIntegrationEventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        public async Task Handle(UserUpdatedIntegrationEvent @event)
        {
            var subscription = new UserSubscription(@event.UserId);
            var method = UserUpdatedMethod.WithArgs(@event.UserId, @event.Name, @event.Tag);

            await _publisherService.PublishAsync(subscription, method);
        }
    }
}
