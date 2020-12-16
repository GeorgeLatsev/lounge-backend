namespace Lounge.Services.Notifications.API.Notifications.Subscriptions
{
    public class UserSubscription : BaseSubscription
    {
        public UserSubscription(string userId)
            : base($":user:{userId}")
        { }
    }
}
