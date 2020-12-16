using Lounge.BuildingBlocks.EventBus.Events;

namespace Lounge.Services.Notifications.API.IntegrationEvents.Users.Events
{
    public class UserSettingsUpdatedIntegrationEvent : IntegrationEvent
    {
        public UserSettingsUpdatedIntegrationEvent(string userId, ThemeEnum theme)
        {
            UserId = userId;
            Theme = theme;
        }

        public string UserId { get; }

        public ThemeEnum Theme { get; }

        public enum ThemeEnum
        {
            Light = 0,
            Dark = 1
        }
    }
}
