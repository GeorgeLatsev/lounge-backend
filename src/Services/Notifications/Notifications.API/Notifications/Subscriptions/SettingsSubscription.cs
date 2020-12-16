namespace Lounge.Services.Notifications.API.Notifications.Subscriptions
{
    public class SettingsSubscription : BaseSubscription
    {
        public SettingsSubscription(string userId)
            : base($":user:settings:{userId}")
        { }
    }
}
