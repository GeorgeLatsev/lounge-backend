using System.Threading.Tasks;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;

namespace Lounge.Services.Notifications.API.Notifications
{
    public interface ISubscriptionService
    {
        Task SubscribeAsync(string connectionId, BaseSubscription subscription);

        Task UnsubscribeAsync(string connectionId, BaseSubscription subscription);
    }
}
