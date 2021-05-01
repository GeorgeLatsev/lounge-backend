using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using System.Collections.Generic;
using System.Linq;

namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public class RoomCreatedMethod : BaseMethod
    {
        private RoomCreatedMethod(RoomCreatedMethodPayload data)
            : base(data)
        { }

        public static RoomCreatedMethod WithArgs(
            int roomId, int type, string name, string ownerId,
            ICollection<GroupRoomMemberAddedIntegrationEvent.Member> members)
        {
            var payload = new RoomCreatedMethodPayload
            {
                Id = roomId,
                Type = type,
                Name = name,
                OwnerId = ownerId,
                Members = members
                    .Select(m => new MemberPayload
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Tag = m.Tag
                    })
                    .ToArray()
            };

            return new RoomCreatedMethod(payload);
        }

        public static RoomCreatedMethod WithArgs(
            int roomId, int type, string name, string ownerId,
            ICollection<RoomCreatedIntegrationEvent.Member> members)
        {
            var payload = new RoomCreatedMethodPayload
            {
                Id = roomId,
                Type = type,
                Name = name,
                OwnerId = ownerId,
                Members = members
                    .Select(m => new MemberPayload
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Tag = m.Tag
                    })
                    .ToArray()
            };

            return new RoomCreatedMethod(payload);
        }

        private class RoomCreatedMethodPayload
        {
            public int Id { get; set; }

            public int Type { get; set; }

            public string Name { get; set; }

            public string OwnerId { get; set; }

            public ICollection<MemberPayload> Members { get; set; }
        }

        private class MemberPayload
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Tag { get; set; }
        }
    }
}
