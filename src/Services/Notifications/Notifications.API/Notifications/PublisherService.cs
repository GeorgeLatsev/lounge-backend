using Lounge.Services.Notifications.API.Hubs;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.Notifications
{
    public class PublisherService : IPublisherService
    {
        private readonly IHubContext<NotificationsHub> _hubContext;

        public PublisherService(IHubContext<NotificationsHub> hubContext)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        public async Task PublishAsync(BaseSubscription subscription, BaseMethod method)
        {
            await _hubContext
                .Clients
                .Group(subscription.GetSubscriptionString())
                .SendCoreAsync(method.GetName(), method.GetArgs());
        }

        public async Task PublishAsync(string connectionId, BaseMethod method)
        {
            await _hubContext
                .Clients
                .Client(connectionId)
                .SendCoreAsync(method.GetName(), method.GetArgs());
        }
    }
}
