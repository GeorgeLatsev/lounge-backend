using Lounge.BuildingBlocks.EventBus.Events;

namespace Lounge.Services.Users.Services.Rooms.IntegrationEvents
{
    public class GroupRoomMemberRemovedIntegrationEvent : IntegrationEvent
    {
        public GroupRoomMemberRemovedIntegrationEvent(int roomId, string memberId)
        {
            RoomId = roomId;
            MemberId = memberId;
        }

        public int RoomId { get; }

        public string MemberId { get; }
    }
}
