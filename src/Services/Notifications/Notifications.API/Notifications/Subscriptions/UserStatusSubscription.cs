namespace Lounge.Services.Notifications.API.Notifications.Subscriptions
{
    public class UserStatusSubscription : BaseSubscription
    {
        public UserStatusSubscription(string userId)
            : base($":user:status:{userId}")
        { }
    }
}
