namespace Lounge.Services.Notifications.API.Notifications.Subscriptions
{
    public class BaseSubscription
    {
        private readonly string _subscriptionString;

        protected BaseSubscription(string subscriptionString)
        {
            _subscriptionString = subscriptionString;
        }

        public string GetSubscriptionString() => _subscriptionString;
    }
}
