using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcNotifcations;
using Lounge.Services.Notifications.API.Notifications;
using Lounge.Services.Notifications.API.Notifications.Methods;
using Lounge.Services.Notifications.API.Notifications.Subscriptions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.Grpc
{
    public class NotificationsGrpcService : NotificationsGrpc.NotificationsGrpcBase
    {
        private readonly IUserConnectionService _userConnectionService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPublisherService _publisherService;
        private readonly ILogger<NotificationsGrpcService> _logger;

        public NotificationsGrpcService(
            IUserConnectionService userConnectionService,
            ISubscriptionService subscriptionService,
            IPublisherService publisherService,
            ILogger<NotificationsGrpcService> logger)
        {
            _userConnectionService =
                userConnectionService ?? throw new ArgumentNullException(nameof(userConnectionService));
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<Empty> SubscribeToRoomsUpdates(RoomsUpdatesRequest request, ServerCallContext context)
        {
            _logger.LogInformation(
                "Begin grpc call from method {Method} to subscribe to Rooms {RoomsIds} with connectionId {ConnectionId}",
                context.Method,
                string.Join(',', request.Rooms.Select(r => r.Id)),
                request.ConnectionId);

            var verifiedConnection =
                await _userConnectionService.VerifyConnectionAsync(request.UserId, request.ConnectionId);
            if (!verifiedConnection)
            {
                context.Status = new Status(StatusCode.InvalidArgument, "Invalid connection id.");
                return new Empty();
            }

            foreach (var dm in request.Rooms)
            {
                var dmSubscription = new RoomSubscription(dm.Id);
                await _subscriptionService.SubscribeAsync(request.ConnectionId, dmSubscription);

                foreach (var member in dm.MembersIds)
                {
                    var userSubscription = new UserSubscription(member);
                    await _subscriptionService.SubscribeAsync(request.ConnectionId, userSubscription);
                }
            }

            return new Empty();
        }

        public override async Task<Empty> SubscribeToUsersUpdates(ConnectionsUpdatesRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation(
                "Begin grpc call from method {Method} to subscribe to users updates {UsersIds} with connectionId {ConnectionId}",
                context.Method,
                string.Join(',', request.UsersIds),
                request.ConnectionId);

            var verifiedConnection =
                await _userConnectionService.VerifyConnectionAsync(request.UserId, request.ConnectionId);
            if (!verifiedConnection)
            {
                context.Status = new Status(StatusCode.InvalidArgument, "Invalid connection id.");
                return new Empty();
            }

            foreach (var userId in request.UsersIds)
            {
                var userSubscription = new UserSubscription(userId);
                await _subscriptionService.SubscribeAsync(request.ConnectionId, userSubscription);

                var relationshipSubscription = new ConnectionSubscription(request.UserId, userId);
                await _subscriptionService.SubscribeAsync(request.ConnectionId, relationshipSubscription);
            }

            return new Empty();
        }

        public override async Task<Empty> SubscribeToUsersStatusUpdates(ConnectionsUpdatesRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation(
                "Begin grpc call from method {Method} to subscribe to users status updates {UsersIds} with connectionId {ConnectionId}",
                context.Method,
                string.Join(',', request.UsersIds),
                request.ConnectionId);

            var verifiedConnection =
                await _userConnectionService.VerifyConnectionAsync(request.UserId, request.ConnectionId);
            if (!verifiedConnection)
            {
                context.Status = new Status(StatusCode.InvalidArgument, "Invalid connection id.");
                return new Empty();
            }

            foreach (var userId in request.UsersIds)
            {
                var statusSubscription = new UserStatusSubscription(userId);
                await _subscriptionService.SubscribeAsync(request.ConnectionId, statusSubscription);

                var isOnline = await _userConnectionService.HasConnectionsAsync(userId);
                var method = UserStatusUpdatedMethod.WithArgs(userId,
                    isOnline ? UserStatusUpdatedMethod.UserStatus.Online : UserStatusUpdatedMethod.UserStatus.Offline);

                await _publisherService.PublishAsync(request.ConnectionId, method);
            }

            return new Empty();
        }
    }
}
