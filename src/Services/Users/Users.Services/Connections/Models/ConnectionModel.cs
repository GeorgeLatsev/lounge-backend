using Lounge.Services.Users.Models.ConnectionEntities;

namespace Lounge.Services.Users.Services.Connections.Models
{
    public class ConnectionModel
    {
        public string UserId { get; set; }

        public string OtherId { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public string Notes { get; set; }

        public RelationshipEnum Relationship { get; set; }


        public static ConnectionModel MapFrom(Connection connection)
        {
            return new ConnectionModel
            {
                UserId = connection.UserId,
                OtherId = connection.OtherId,
                Name = connection.OtherUser.Name,
                Tag = connection.OtherUser.Tag,
                Notes = connection.Notes,
                Relationship = connection.Relationship
            };
        }
    }
}
