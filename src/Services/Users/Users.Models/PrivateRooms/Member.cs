using Lounge.Services.Users.Models.Users;

namespace Lounge.Services.Users.Models.PrivateRooms
{
    public class Member
    {
        public int RoomId { get; set; }
        public PrivateRoom Room { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
