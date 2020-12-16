namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public class UserUpdatedMethod : BaseMethod
    {
        private UserUpdatedMethod(UserUpdatedMethodPayload data)
            : base(data)
        { }

        public static UserUpdatedMethod WithArgs(string id, string name, string tag)
        {
            var payload = new UserUpdatedMethodPayload
            {
                Id = id,
                Name = name,
                Tag = tag
            };

            return new UserUpdatedMethod(payload);
        }

        private class UserUpdatedMethodPayload
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Tag { get; set; }
        }
    }
}
