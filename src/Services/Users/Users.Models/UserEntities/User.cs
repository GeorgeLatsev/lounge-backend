using System.Collections.Generic;
using Lounge.Services.Users.Models.ConnectionEntities;
using Lounge.Services.Users.Models.RoomEntities;

namespace Lounge.Services.Users.Models.UserEntities
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        public string Tag { get; set; }

        public Settings Settings { get; set; } = new Settings();

        public ICollection<Connection> Connections { get; set; } = new HashSet<Connection>();

        public ICollection<Member> PrivateRooms { get; set; } = new HashSet<Member>();
    }
}