using System.Threading.Tasks;
using Lounge.Services.Notifications.API.Grpc.Clients.Users.Models;

namespace Lounge.Services.Notifications.API.Grpc.Clients.Users
{
    public interface IUsersGrpcService
    {
        Task<UserModel> GetUserAsync(string id);
    }
}
