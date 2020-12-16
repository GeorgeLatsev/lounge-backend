using Lounge.BuildingBlocks.EventBus.Events;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.Events
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
