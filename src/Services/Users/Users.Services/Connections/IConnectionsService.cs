using Lounge.Services.Users.Services.Connections.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lounge.Services.Users.Services.Connections
{
    public interface IConnectionsService
    {
        Task<Result<IEnumerable<ConnectionModel>>> GetAllAsync(string userId);

        Task<Result<ConnectionModel>> GetAsync(string userId, string otherId);

        Task<Result<ConnectionModel>> GetByDisplayNameAsync(string userId, string displayName);

        Task<Result> UpdateAsync(string userId, string otherId, ConnectionUpdateModel model);
    }
}
