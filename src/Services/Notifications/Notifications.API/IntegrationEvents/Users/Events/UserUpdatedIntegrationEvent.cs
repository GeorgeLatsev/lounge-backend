using Lounge.BuildingBlocks.EventBus.Events;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.Events
{
    public class UserUpdatedIntegrationEvent : IntegrationEvent
    {
        public UserUpdatedIntegrationEvent(string userId, string name, string tag)
        {
            UserId = userId;
            Name = name;
            Tag = tag;
        }

        public string UserId { get; }

        public string Name { get; }

        public string Tag { get; }
    }
}
