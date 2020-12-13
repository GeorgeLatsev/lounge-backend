using System.Threading.Tasks;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;

namespace Lounge.Services.Notifications.API.Notifications
{
    public interface IPublisherService
    {
        Task PublishAsync(BaseSubscription subscription, BaseMethod method);

        Task PublishAsync(string connectionId, BaseMethod method);
    }
}
