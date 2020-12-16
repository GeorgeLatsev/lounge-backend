namespace Lounge.Services.Notifications.API.Notifications.Subscriptions
{
    public class ConnectionSubscription : BaseSubscription
    {
        public ConnectionSubscription(string userId, string otherId)
            : base($":user:connection:{userId}:{otherId}")
        { }
    }
}