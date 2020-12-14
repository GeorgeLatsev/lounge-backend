namespace Lounge.Services.Notifications.API.Grpc.Clients.Users.Models
{
    public class SettingsModel
    {
        public Theme Theme { get; set; }
    }

    public enum Theme
    {
        Light = 0,
        Dark = 1
    }
}
