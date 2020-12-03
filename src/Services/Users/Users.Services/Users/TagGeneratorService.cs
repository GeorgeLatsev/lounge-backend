using System;
using System.Text;
using System.Threading.Tasks;
using Lounge.Services.Users.Infrastructure.Data;
using Lounge.Services.Users.Models.UserEntities;
using Microsoft.EntityFrameworkCore;

namespace Lounge.Services.Users.Services.Users
{
    public class TagGeneratorService : ITagGeneratorService
    {
        private readonly UsersContext _context;

        public TagGeneratorService(UsersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<string>> GenerateAsync(string userId, string name)
        {
            var seed = (int)DateTime.Now.Ticks + name.GetHashCode() + userId.GetHashCode();
            var rng = new Random(seed);

            var usernameOccurrences = await _context.Set<User>()
                .CountAsync(u => u.Name == name);

            if (usernameOccurrences >= 256)
            {
                var error = UsersServiceErrors.NameOccurrencesLimitExceeded(name);
                return Result<string>.Failure(error);
            }

            var tag = string.Empty;
            var tagExists = true;

            while (tagExists)
            {
                var randomNumber = rng.Next(0, int.MaxValue);

                tag = CreateTag(usernameOccurrences, randomNumber);

                tagExists = await _context.Set<User>()
                    .AnyAsync(u => u.Name == name && u.Tag == tag);
            }

            return Result<string>.SuccessWith(tag);
        }

        private string CreateTag(int part1, int part2)
        {
            var str1 = EncodeIntAsString(part1, ModelConstants.User.MaxTagLength / 2);
            var str2 = EncodeIntAsString(part2, ModelConstants.User.MaxTagLength / 2);

            var tag = string.Empty + str1[0] + str2[0] + str1[1] + str2[1];

            return tag;
        }

        private string EncodeIntAsString(int input, int length)
        {
            var chars = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

            var result = new StringBuilder(length);

            while (input > 0 && length > 0)
            {
                var moduloResult = input % chars.Length;
                input /= chars.Length;
                result.Insert(0, chars[moduloResult]);
                length--;
            }

            if (length > 0)
            {
                result.Insert(0, new string(chars[0], length));
            }

            return result.ToString();
        }
    }
}
