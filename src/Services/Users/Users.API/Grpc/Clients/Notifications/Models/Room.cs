namespace Lounge.Services.Users.API.Grpc.Clients.Notifications.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string[] MembersIds { get; set; }
    }
}
