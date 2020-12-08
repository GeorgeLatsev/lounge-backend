using System.Threading.Tasks;
using Lounge.Services.Users.Services.Rooms.Models;

namespace Lounge.Services.Users.Services.Rooms
{
    public interface IGroupRoomsService
    {
        Task<Result> AddRecipientAsync(int roomId, string userToAddId, string callerId);

        Task<Result> RemoveRecipientAsync(int roomId, string userToRemoveId, string callerId);

        Task<Result> UpdateAsync(int roomId, GroupRoomUpdateModel model, string callerId);
    }
}
