using Lounge.BuildingBlocks.EventBus.Events;

namespace Lounge.Services.Users.Services.Rooms.IntegrationEvents
{
    public class GroupRoomDeletedIntegrationEvent : IntegrationEvent
    {
        public GroupRoomDeletedIntegrationEvent(int roomId)
        {
            RoomId = roomId;
        }

        public int RoomId { get; }
    }
}
