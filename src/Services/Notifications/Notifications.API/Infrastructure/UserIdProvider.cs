using Microsoft.AspNetCore.SignalR;

namespace Lounge.Services.Notifications.API.Infrastructure
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
            => connection.User?.FindFirst("sub")?.Value;
    }
}
