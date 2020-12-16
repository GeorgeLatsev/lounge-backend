using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using System;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling
{
    public class UserSettingsUpdatedIntegrationEventHandler : IIntegrationEventHandler<UserSettingsUpdatedIntegrationEvent>
    {
        private readonly IPublisherService _publisherService;

        public UserSettingsUpdatedIntegrationEventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        public async Task Handle(UserSettingsUpdatedIntegrationEvent @event)
        {
            var subscription = new SettingsSubscription(@event.UserId);
            var method = SettingsUpdatedMethod.WithArgs((int)@event.Theme);

            await _publisherService.PublishAsync(subscription, method);
        }
    }
}
