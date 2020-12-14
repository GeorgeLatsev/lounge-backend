using Lounge.Services.Notifications.API.Config;
using Lounge.Services.Notifications.API.Grpc.Clients.Users.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Lounge.Services.Users.API.Proto;

namespace Lounge.Services.Notifications.API.Grpc.Clients.Users
{
    public class UsersGrpcService : IUsersGrpcService
    {
        private readonly UrlsConfig _urls;
        public readonly HttpClient _httpClient;

        public UsersGrpcService(
            IOptions<UrlsConfig> config,
            HttpClient httpClient)
        {
            _urls = config.Value;
            _httpClient = httpClient;
        }

        public async Task<UserModel> GetUserAsync(string id)
        {
            return await GrpcCallerService.CallService(_urls.GrpcUsers, async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);

                var response = await client.GetUserAsync(new UserRequest { Id = id });

                return MapToUserData(response);
            });
        }

        private static UserModel MapToUserData(UserResponse response)
        {
            return new UserModel
            {
                Id = response.Id,
                Name = response.Name,
                Tag = response.Tag,
                Settings = MapSettings(response.Settings)
            };
        }

        private static SettingsModel MapSettings(SettingsResponse settings)
        {
            return new SettingsModel
            {
                Theme = (Models.Theme)settings.Theme
            };
        }
    }
}
