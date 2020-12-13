using System;
using System.Threading.Tasks;
using Lounge.Services.Notifications.API.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Lounge.Services.Notifications.API.Hubs
{
    [Authorize]
    public class NotificationsHub : Hub
    {
        private readonly IUserConnectionService _userConnectionService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPublisherService _publisherService;

        public NotificationsHub(
            IUserConnectionService userConnectionService,
            ISubscriptionService subscriptionService,
            IPublisherService publisherService)
        {
            _userConnectionService = userConnectionService ?? throw new ArgumentNullException(nameof(userConnectionService));
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        public override async Task OnConnectedAsync()
        {
            await _userConnectionService.AddConnectionAsync(Context.UserIdentifier, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _userConnectionService.RemoveConnectionAsync(Context.UserIdentifier, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
