using System;
using System.Threading.Tasks;
using Lounge.Services.Notifications.API.Grpc.Clients.Users;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
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
        private readonly IUsersGrpcService _usersService;

        public NotificationsHub(
            IUserConnectionService userConnectionService,
            ISubscriptionService subscriptionService,
            IPublisherService publisherService,
            IUsersGrpcService usersService)
        {
            _userConnectionService = userConnectionService ?? throw new ArgumentNullException(nameof(userConnectionService));
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _usersService.GetUserAsync(Context.UserIdentifier);

            var userSubscription = new UserSubscription(Context.UserIdentifier);
            await _subscriptionService.SubscribeAsync(Context.ConnectionId, userSubscription);

            var settingsSubscription = new SettingsSubscription(Context.UserIdentifier);
            await _subscriptionService.SubscribeAsync(Context.ConnectionId, settingsSubscription);

            var readyMethod = ReadyMethod.WithArgs(user.Id, user.Name, user.Tag, (int)user.Settings.Theme);
            await _publisherService.PublishAsync(Context.ConnectionId, readyMethod);

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
