using Lounge.BuildingBlocks.EventBus.Events;
using System.Collections.Generic;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.Events
{
    public class GroupRoomMemberAddedIntegrationEvent : IntegrationEvent
    {
        public GroupRoomMemberAddedIntegrationEvent(int roomId, GroupRoom room, Member addedMember)
        {
            RoomId = roomId;
            Room = room;
            AddedMember = addedMember;
        }

        public int RoomId { get; }

        public GroupRoom Room { get; }

        public Member AddedMember { get; }

        public class GroupRoom
        {
            public string Name { get; set; }

            public string OwnerId { get; set; }

            public ICollection<Member> Members { get; set; }
        }

        public class Member
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Tag { get; set; }
        }
    }
}
