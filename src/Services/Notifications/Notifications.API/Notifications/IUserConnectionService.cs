using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lounge.Services.Notifications.API.Notifications
{
    public interface IUserConnectionService
    {
        Task AddConnectionAsync(string userId, string connectionId);

        Task<ICollection<string>> GetConnectionsAsync(string userId);

        Task<bool> HasConnectionsAsync(string userId);

        Task RemoveConnectionAsync(string userId, string connectionId);

        Task<bool> VerifyConnectionAsync(string userId, string connectionId);
    }
}
