using Lounge.BuildingBlocks.EventBus.Events;
using System.Collections.Generic;
using System.Linq;
using Lounge.Services.Users.Models.UserEntities;
using RoomEntities = Lounge.Services.Users.Models.RoomEntities;

namespace Lounge.Services.Users.Services.Rooms.IntegrationEvents
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

        public static GroupRoomMemberAddedIntegrationEvent For(RoomEntities.GroupRoom room, User addedMember)
        {
            var members = room.Members
                .Select(m => new Member
                {
                    Id = m.UserId,
                    Name = m.User.Name,
                    Tag = m.User.Tag
                })
                .ToArray();

            var roomData = new GroupRoom
            {
                Name = room.Name,
                OwnerId = room.OwnerId,
                Members = members
            };

            var memberData = new Member
            {
                Id = addedMember.Id,
                Name = addedMember.Name,
                Tag = addedMember.Tag
            };

            return new GroupRoomMemberAddedIntegrationEvent(room.Id, roomData, memberData);
        }
    }
}
