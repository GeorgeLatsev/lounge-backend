using Lounge.BuildingBlocks.EventBus.Events;

namespace Lounge.Services.Users.Services.Connections.IntegrationEvents
{
    public class ConnectionCreatedIntegrationEvent : IntegrationEvent
    {
        public ConnectionCreatedIntegrationEvent(string userId, string otherId, string name, string tag)
        {
            UserId = userId;
            OtherId = otherId;
            Name = name;
            Tag = tag;
        }

        public string UserId { get; }

        public string OtherId { get; }

        public string Name { get; }

        public string Tag { get; }
    }
}
