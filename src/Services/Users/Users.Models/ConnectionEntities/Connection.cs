using Lounge.Services.Users.Models.UserEntities;

namespace Lounge.Services.Users.Models.ConnectionEntities
{
    public class Connection
    {
        public string UserId { get; set; }

        public string OtherId { get; set; }

        public User OtherUser { get; set; }

        public string Notes { get; set; } = string.Empty;

        public RelationshipEnum Relationship { get; set; }
    }
}
