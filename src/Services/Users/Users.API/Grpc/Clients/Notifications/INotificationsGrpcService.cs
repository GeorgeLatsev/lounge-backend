using Lounge.Services.Users.API.Grpc.Clients.Notifications.Models;
using System.Threading.Tasks;

namespace Lounge.Services.Users.API.Grpc.Clients.Notifications
{
    public interface INotificationsGrpcService
    {
        Task SubscribeToRoomsUpdatesAsync(string userId, string connectionId, params Room[] rooms);

        Task SubscribeToUsersUpdatesAsync(string userId, string connectionId, params string[] usersIds);

        Task SubscribeToUsersStatusUpdatesAsync(string userId, string connectionId, params string[] usersIds);
    }
}
