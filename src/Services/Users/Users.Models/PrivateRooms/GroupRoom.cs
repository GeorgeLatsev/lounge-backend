namespace Lounge.Services.Users.Models.PrivateRooms
{
    public class GroupRoom : PrivateRoom
    {
        public string Name { get; set; }

        public string OwnerId { get; set; }
    }
}
