using GrpcNotifcations;
using Lounge.Services.Users.API.Config;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Room = Lounge.Services.Users.API.Grpc.Clients.Notifications.Models.Room;

namespace Lounge.Services.Users.API.Grpc.Clients.Notifications
{
    public class NotificationsGrpcService : INotificationsGrpcService
    {
        private readonly UrlsConfig _urls;
        public readonly HttpClient _httpClient;

        public NotificationsGrpcService(
            IOptions<UrlsConfig> config,
            HttpClient httpClient)
        {
            _urls = config.Value;
            _httpClient = httpClient;
        }

        public async Task SubscribeToRoomsUpdatesAsync(string userId, string connectionId, params Room[] rooms)
        {
            await GrpcCallerService.CallService(_urls.GrpcNotifications, async channel =>
            {
                var client = new NotificationsGrpc.NotificationsGrpcClient(channel);

                var request = new RoomsUpdatesRequest
                {
                    UserId = userId,
                    ConnectionId = connectionId
                };

                foreach (var room in rooms)
                {
                    var roomModel = new GrpcNotifcations.Room { Id = room.Id };
                    roomModel.MembersIds.AddRange(room.MembersIds);

                    request.Rooms.Add(roomModel);
                }

                await client.SubscribeToRoomsUpdatesAsync(request);
            });
        }

        public async Task SubscribeToUsersUpdatesAsync(string userId, string connectionId, params string[] usersIds)
        {
            await GrpcCallerService.CallService(_urls.GrpcNotifications, async channel =>
            {
                var client = new NotificationsGrpc.NotificationsGrpcClient(channel);

                var request = new ConnectionsUpdatesRequest
                {
                    UserId = userId,
                    ConnectionId = connectionId
                };
                request.UsersIds.AddRange(usersIds);

                await client.SubscribeToUsersUpdatesAsync(request);
            });
        }

        public async Task SubscribeToUsersStatusUpdatesAsync(string userId, string connectionId, params string[] usersIds)
        {
            await GrpcCallerService.CallService(_urls.GrpcNotifications, async channel =>
            {
                var client = new NotificationsGrpc.NotificationsGrpcClient(channel);

                var request = new ConnectionsUpdatesRequest
                {
                    UserId = userId,
                    ConnectionId = connectionId
                };
                request.UsersIds.AddRange(usersIds);

                await client.SubscribeToUsersStatusUpdatesAsync(request);
            });
        }
    }

}
