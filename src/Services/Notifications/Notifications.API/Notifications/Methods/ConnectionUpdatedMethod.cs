namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public class ConnectionUpdatedMethod : BaseMethod
    {
        private ConnectionUpdatedMethod(ConnectionUpdatedMethodPayload data)
            : base(data)
        { }

        public static ConnectionUpdatedMethod WithArgs(string otherId, string notes, int relationship)
        {
            var payload = new ConnectionUpdatedMethodPayload
            {
                OtherId = otherId,
                Notes = notes,
                Relationship = relationship
            };

            return new ConnectionUpdatedMethod(payload);
        }

        private class ConnectionUpdatedMethodPayload
        {
            public string OtherId { get; set; }

            public string Notes { get; set; }

            public int Relationship { get; set; }
        }
    }
}
