using Lounge.BuildingBlocks.EventBus.Events;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.Events
{
    public class ConnectionUpdatedIntegrationEvent : IntegrationEvent
    {
        public ConnectionUpdatedIntegrationEvent(string userId, string otherId, string notes, RelationshipEnum relationship)
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

        public enum RelationshipEnum
        {
            None = 0,
            IncomingRequest = 1,
            OutgoingRequest = 2,
            Friend = 3,
            Blocked = 4,
            BeingBlocked = 5
        }
    }
}
