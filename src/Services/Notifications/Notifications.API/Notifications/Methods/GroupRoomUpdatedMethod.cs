namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public class GroupRoomUpdatedMethod : BaseMethod
    {
        private GroupRoomUpdatedMethod(GroupRoomUpdatedMethodPayload data)
            : base(data)
        { }

        public static GroupRoomUpdatedMethod WithArgs(int id, string name, string ownerId)
        {
            var payload = new GroupRoomUpdatedMethodPayload
            {
                Id = id,
                Name = name,
                OwnerId = ownerId
            };

            return new GroupRoomUpdatedMethod(payload);
        }


        private class GroupRoomUpdatedMethodPayload
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string OwnerId { get; set; }
        }
    }
}
