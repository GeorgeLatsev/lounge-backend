using System.Threading.Tasks;
using Lounge.Services.Users.Models.UserEntities;

namespace Lounge.Services.Users.Services.Users
{
    public interface ISettingsService
    {
        Task<Result<Settings>> GetAsync(string userId);

        Task<Result> UpdateAsync(string userId, Settings settings);
    }
}
