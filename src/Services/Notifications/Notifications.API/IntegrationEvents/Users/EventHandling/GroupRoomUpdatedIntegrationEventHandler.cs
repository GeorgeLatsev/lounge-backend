using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using System;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling
{
    public class GroupRoomUpdatedIntegrationEventHandler : IIntegrationEventHandler<GroupRoomUpdatedIntegrationEvent>
    {
        private readonly IPublisherService _publisherService;

        public GroupRoomUpdatedIntegrationEventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        public async Task Handle(GroupRoomUpdatedIntegrationEvent @event)
        {
            var roomSubscription = new PrivateRoomSubscription(@event.RoomId);
            var method = GroupRoomUpdatedMethod.WithArgs(@event.RoomId, @event.Name, @event.OwnerId);

            await _publisherService.PublishAsync(roomSubscription, method);
        }
    }
}
