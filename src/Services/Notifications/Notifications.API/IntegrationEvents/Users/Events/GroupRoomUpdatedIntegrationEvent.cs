using Lounge.BuildingBlocks.EventBus.Events;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.Events
{
    public class GroupRoomUpdatedIntegrationEvent : IntegrationEvent
    {
        public GroupRoomUpdatedIntegrationEvent(int roomId, string name, string ownerId)
        {
            RoomId = roomId;
            Name = name;
            OwnerId = ownerId;
        }

        public int RoomId { get; }

        public string Name { get; }

        public string OwnerId { get; }
    }
}
