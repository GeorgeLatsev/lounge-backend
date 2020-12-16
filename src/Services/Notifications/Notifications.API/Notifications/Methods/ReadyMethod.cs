namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public class ReadyMethod : BaseMethod
    {
        private ReadyMethod(ReadyMethodPayload data)
            : base(data)
        { }

        public static ReadyMethod WithArgs(string id, string name, string tag, int theme)
        {
            var payload = new ReadyMethodPayload
            {
                Id = id,
                Name = name,
                Tag = tag,
                Settings = new UserSettingsPayload
                {
                    Theme = theme
                }
            };

            return new ReadyMethod(payload);
        }

        private class ReadyMethodPayload
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Tag { get; set; }

            public UserSettingsPayload Settings { get; set; }
        }

        private class UserSettingsPayload
        {
            public int Theme { get; set; }
        }
    }
}
