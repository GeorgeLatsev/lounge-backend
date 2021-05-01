using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using System;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling
{
    public class RoomCreatedIntegrationEventHandler : IIntegrationEventHandler<RoomCreatedIntegrationEvent>
    {
        private readonly IUserConnectionService _userConnectionService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPublisherService _publisherService;

        public RoomCreatedIntegrationEventHandler(
            IUserConnectionService userConnectionService,
            ISubscriptionService subscriptionService,
            IPublisherService publisherService)
        {
            _userConnectionService = userConnectionService ?? throw new ArgumentNullException(nameof(userConnectionService));
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        public async Task Handle(RoomCreatedIntegrationEvent @event)
        {
            var roomSubscription = new RoomSubscription(@event.RoomId);

            var roomType = @event.Members.Count > 2 ? RoomCreatedIntegrationEvent.RoomType.Group : RoomCreatedIntegrationEvent.RoomType.Private;
            var method = RoomCreatedMethod.WithArgs(@event.RoomId, (int)roomType, null, @event.OwnerId, @event.Members);

            foreach (var recipient in @event.Members)
            {
                var connections = await _userConnectionService.GetConnectionsAsync(recipient.Id);
                foreach (var connectionId in connections)
                {
                    await _subscriptionService.SubscribeAsync(connectionId, roomSubscription);
                }
            }

            await _publisherService.PublishAsync(roomSubscription, method);
        }
    }
}
