using Lounge.BuildingBlocks.EventBus.Events;
using Lounge.Services.Users.Models.Users;

namespace Lounge.Services.Users.Services.Connections.IntegrationEvents
{
    public class ConnectionUpdatedntegrationEvent : IntegrationEvent
    {
        public ConnectionUpdatedntegrationEvent(string userId, string otherId, string notes, RelationshipEnum relationship)
        {
            UserId = userId;
            OtherId = otherId;
            Notes = notes;
            Relationship = relationship;
        }

        public string UserId { get; }

        public string OtherId { get; }

        public string Notes { get; }

        public RelationshipEnum Relationship { get; }
    }
}
