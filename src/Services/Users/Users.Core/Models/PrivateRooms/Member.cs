using Lounge.Services.Users.Core.Models.Users;

namespace Lounge.Services.Users.Core.Models.PrivateRooms
{
    public class Member
    {
        public int RoomId { get; set; }
        public PrivateRoom Room { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
