using Lounge.Services.Users.Models.Users;
using System.Threading.Tasks;

namespace Lounge.Services.Users.Services.Users
{
    public interface ISettingsService
    {
        Task<Result<Settings>> GetAsync(string userId);

        Task<Result> UpdateAsync(string userId, Settings settings);
    }
}
