namespace Lounge.Services.Users.Services.Users.Models
{
    public class UserUpdateModel
    {
        public string Name { get; set; }

        public static UserUpdateModel From(UserModel user)
        {
            return new UserUpdateModel
            {
                Name = user.Name
            };
        }
    }
}
