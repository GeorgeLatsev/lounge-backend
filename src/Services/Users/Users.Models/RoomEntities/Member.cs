using Lounge.Services.Users.Models.UserEntities;

namespace Lounge.Services.Users.Models.RoomEntities
{
    public class Member
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
