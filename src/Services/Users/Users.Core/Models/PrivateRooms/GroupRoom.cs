namespace Lounge.Services.Users.Core.Models.PrivateRooms
{
    public class GroupRoom : PrivateRoom
    {
        public string Name { get; set; }

        public string OwnerId { get; set; }
    }
}
