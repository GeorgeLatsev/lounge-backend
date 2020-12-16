using Lounge.BuildingBlocks.EventBus.Events;
using System.Collections.Generic;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.Events
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

        public enum RoomType
        {
            Private = 1,
            Group = 2
        }
    }
}
