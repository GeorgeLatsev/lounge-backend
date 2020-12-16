namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public class ConnectionCreatedMethod : BaseMethod
    {
        private ConnectionCreatedMethod(ConnectionCreatedMethodPayload data)
            : base(data)
        { }

        public static ConnectionCreatedMethod WithArgs(string otherId, string name, string tag)
        {
            var payload = new ConnectionCreatedMethodPayload
            {
                OtherId = otherId,
                Name = name,
                Tag = tag
            };

            return new ConnectionCreatedMethod(payload);
        }

        private class ConnectionCreatedMethodPayload
        {
            public string OtherId { get; set; }

            public string Name { get; set; }

            public string Tag { get; set; }
        }
    }
}
