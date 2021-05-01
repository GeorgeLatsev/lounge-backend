namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public class UserStatusUpdatedMethod : BaseMethod
    {
        private UserStatusUpdatedMethod(UserStatusUpdatedMethodPayload data)
            : base(data)
        { }

        public static UserStatusUpdatedMethod WithArgs(string id, UserStatus status)
        {
            var payload = new UserStatusUpdatedMethodPayload
            {
                Id = id,
                Status = status
            };

            return new UserStatusUpdatedMethod(payload);
        }

        private class UserStatusUpdatedMethodPayload
        {
            public string Id { get; set; }

            public UserStatus Status { get; set; }
        }

        public enum UserStatus
        {
            Unknown = 0,
            Offline = 1,
            Online = 2
        }
    }
}
