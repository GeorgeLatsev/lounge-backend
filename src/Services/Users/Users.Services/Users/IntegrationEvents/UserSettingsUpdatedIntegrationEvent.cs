using Lounge.BuildingBlocks.EventBus.Events;
using Lounge.Services.Users.Models.UserEntities;

namespace Lounge.Services.Users.Services.Users.IntegrationEvents
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
    }
}
