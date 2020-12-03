using Lounge.Services.Users.Models.Users;

namespace Lounge.Services.Users.Services.Connections.Models
{
    public class ConnectionUpdateModel
    {
        public string Notes { get; set; }

        public RelationshipEnum Relationship { get; set; }


        public static ConnectionUpdateModel From(ConnectionModel connection)
        {
            return new ConnectionUpdateModel
            {
                Notes = connection.Notes,
                Relationship = connection.Relationship
            };
        }
    }
}
