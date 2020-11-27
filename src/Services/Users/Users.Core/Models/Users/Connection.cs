namespace Lounge.Services.Users.Core.Models.Users
{
    public class Connection
    {
        public string UserId { get; set; }

        public string OtherUserId { get; set; }

        public User OtherUser { get; set; }

        public string Notes { get; set; }

        public RelationshipEnum Relationship { get; set; }
    }
}
