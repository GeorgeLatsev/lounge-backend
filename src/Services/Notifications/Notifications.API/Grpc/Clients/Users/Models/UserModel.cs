namespace Lounge.Services.Notifications.API.Grpc.Clients.Users.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public SettingsModel Settings { get; set; }
    }
}
