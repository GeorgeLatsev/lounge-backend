using Lounge.Services.Users.Models.UserEntities;

namespace Lounge.Services.Users.Services.Users.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public static UserModel MapFrom(User user)
        {
            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Tag = user.Tag
            };
        }
    }
}
