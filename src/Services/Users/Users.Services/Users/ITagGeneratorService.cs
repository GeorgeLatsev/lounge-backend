using System.Threading.Tasks;

namespace Lounge.Services.Users.Services.Users
{
    public interface ITagGeneratorService
    {
        Task<Result<string>> GenerateAsync(string userId, string name);
    }
}
