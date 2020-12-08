using Lounge.BuildingBlocks.EventBus.Events;

namespace Lounge.Services.Users.Services.Rooms.IntegrationEvents
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
