using System.Collections.Generic;
using Lounge.Services.Users.Models.PrivateRooms;

namespace Lounge.Services.Users.Models.Users
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        public string Tag { get; set; }

        public Settings Settings { get; set; }

        public ICollection<Connection> Connections { get; set; } = new HashSet<Connection>();

        public ICollection<Member> PrivateRooms { get; set; } = new HashSet<Member>();
    }
}