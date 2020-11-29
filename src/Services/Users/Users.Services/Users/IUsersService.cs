using Lounge.Services.Users.Services.Users.Models;
using System.Threading.Tasks;

namespace Lounge.Services.Users.Services.Users
{
    public interface IUsersService
    {
        Task<Result<UserModel>> CreateAsync(string id);

        Task<Result<UserModel>> GetAsync(string id);

        Task<Result> UpdateAsync(string id, UserUpdateModel model);
    }
}
