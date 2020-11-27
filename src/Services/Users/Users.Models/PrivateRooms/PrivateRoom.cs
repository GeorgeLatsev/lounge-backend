using System.Collections.Generic;

namespace Lounge.Services.Users.Models.PrivateRooms
{
    public class PrivateRoom
    {
        public int Id { get; set; }

        public int? RoomId { get; set; }

        public PrivateRoomType Type { get; set; }

        public ICollection<Member> Members { get; set; } = new HashSet<Member>();
    }
}
