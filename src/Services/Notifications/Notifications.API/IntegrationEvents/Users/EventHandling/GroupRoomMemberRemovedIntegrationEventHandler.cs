using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using System;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling
{
    public class GroupRoomMemberRemovedIntegrationEventHandler : IIntegrationEventHandler<GroupRoomMemberRemovedIntegrationEvent>
    {
        private readonly IPublisherService _publisherService;

        public GroupRoomMemberRemovedIntegrationEventHandler(IPublisherService publisherService)
        {
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        public async Task Handle(GroupRoomMemberRemovedIntegrationEvent @event)
        {
            var roomSubscription = new RoomSubscription(@event.RoomId);
            var method = GroupRoomMemberRemovedMethod.WithArgs(@event.RoomId, @event.MemberId);

            await _publisherService.PublishAsync(roomSubscription, method);
        }
    }

}
