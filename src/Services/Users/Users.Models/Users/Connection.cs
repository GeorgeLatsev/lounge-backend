namespace Lounge.Services.Users.Models.Users
{
    public class Connection
    {
        public string UserId { get; set; }

        public string OtherUserId { get; set; }

        public User OtherUser { get; set; }

        public string Notes { get; set; } = string.Empty;

        public RelationshipEnum Relationship { get; set; }
    }
}
