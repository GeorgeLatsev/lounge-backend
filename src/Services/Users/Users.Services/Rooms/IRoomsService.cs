using System.Collections.Generic;
using System.Threading.Tasks;
using Lounge.Services.Users.Services.Rooms.Models;

namespace Lounge.Services.Users.Services.Rooms
{
    public interface IRoomsService
    {
        Task<Result<RoomModel>> CreateAsync(string userId, string[] othersIds);

        Task<Result<IEnumerable<RoomModel>>> GetAllAsync(string userId);

        Task<Result<RoomModel>> GetAsync(int id);
    }
}
