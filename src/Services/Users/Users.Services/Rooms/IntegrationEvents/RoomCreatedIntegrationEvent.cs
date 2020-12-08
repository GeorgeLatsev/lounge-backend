using Lounge.BuildingBlocks.EventBus.Events;
using Lounge.Services.Users.Models.RoomEntities;
using System.Collections.Generic;
using System.Linq;

namespace Lounge.Services.Users.Services.Rooms.IntegrationEvents
{
    public class RoomCreatedIntegrationEvent : IntegrationEvent
    {
        public RoomCreatedIntegrationEvent(int roomId, RoomType type, string ownerId, ICollection<Member> members)
        {
            RoomId = roomId;
            Type = type;
            OwnerId = ownerId;
            Members = members;
        }

        public int RoomId { get; }

        public RoomType Type { get; }

        public string OwnerId { get; }

        public ICollection<Member> Members { get; }

        public class Member
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Tag { get; set; }
        }

        public static RoomCreatedIntegrationEvent For(Room room)
        {

            string ownerId = null;
            if (room.Type == RoomType.Group)
            {
                ownerId = (room as GroupRoom).OwnerId;
            }

            var members = room.Members
                 .Select(m => m.User)
                 .Select(u => new Member
                 {
                     Id = u.Id,
                     Name = u.Name,
                     Tag = u.Tag
                 })
                 .ToArray();


            return new RoomCreatedIntegrationEvent(room.Id, room.Type, ownerId, members);
        }
    }
}
