using System.Collections.Generic;

namespace Lounge.Services.Users.Models.RoomEntities
{
    public class Room
    {
        public int Id { get; set; }

        public RoomType Type { get; set; }

        public ICollection<Member> Members { get; set; } = new HashSet<Member>();
    }
}
