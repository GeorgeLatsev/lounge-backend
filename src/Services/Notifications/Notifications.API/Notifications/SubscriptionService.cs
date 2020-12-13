using Lounge.Services.Notifications.API.Hubs;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.Notifications
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IHubContext<NotificationsHub> _hubContext;

        public SubscriptionService(IHubContext<NotificationsHub> hubContext, ILogger<SubscriptionService> logger)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        public async Task SubscribeAsync(string connectionId, BaseSubscription subscription)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, subscription.GetSubscriptionString());
        }

        public async Task UnsubscribeAsync(string connectionId, BaseSubscription subscription)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, subscription.GetSubscriptionString());
        }
    }
}
