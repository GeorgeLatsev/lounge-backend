namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public class SettingsUpdatedMethod : BaseMethod
    {
        private SettingsUpdatedMethod(SettingsUpdatedMethodPayload data)
            : base(data)
        { }

        public static SettingsUpdatedMethod WithArgs(int theme)
        {
            var payload = new SettingsUpdatedMethodPayload
            {
                Theme = theme
            };

            return new SettingsUpdatedMethod(payload);
        }

        private class SettingsUpdatedMethodPayload
        {
            public int Theme { get; set; }
        }
    }
}
