using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using System;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling
{
    public class GroupRoomMemberAddedIntegrationEventHandler : IIntegrationEventHandler<GroupRoomMemberAddedIntegrationEvent>
    {
        private readonly IUserConnectionService _userConnectionService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPublisherService _publisherService;

        public GroupRoomMemberAddedIntegrationEventHandler(
            IUserConnectionService userConnectionService,
            ISubscriptionService subscriptionService,
            IPublisherService publisherService)
        {
            _userConnectionService = userConnectionService ?? throw new ArgumentNullException(nameof(userConnectionService));
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        public async Task Handle(GroupRoomMemberAddedIntegrationEvent @event)
        {
            var roomSubscription = new RoomSubscription(@event.RoomId);
            var method = GroupRoomMemberAddedMethod.WithArgs(
                @event.RoomId, @event.AddedMember.Id, @event.AddedMember.Name, @event.AddedMember.Tag);

            await _publisherService.PublishAsync(roomSubscription, method);

            var connectionIds = await _userConnectionService.GetConnectionsAsync(@event.AddedMember.Id);
            foreach (var connectionId in connectionIds)
            {
                await _subscriptionService.SubscribeAsync(connectionId, roomSubscription);

                var createdMethod = RoomCreatedMethod.WithArgs(
                    @event.RoomId, (int)RoomCreatedIntegrationEvent.RoomType.Group, @event.Room.Name, @event.Room.OwnerId, @event.Room.Members);

                await _publisherService.PublishAsync(connectionId, createdMethod);
            }
        }
    }
}