namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public class GroupRoomMemberAddedMethod : BaseMethod
    {
        private GroupRoomMemberAddedMethod(GroupRoomMemberAddedMethodPayload data)
            : base(data)
        { }

        public static GroupRoomMemberAddedMethod WithArgs(int roomId, string memberId, string name, string tag)
        {
            var payload = new GroupRoomMemberAddedMethodPayload
            {
                RoomId = roomId,
                MemberId = memberId,
                Name = name,
                Tag = tag
            };

            return new GroupRoomMemberAddedMethod(payload);
        }


        private class GroupRoomMemberAddedMethodPayload
        {
            public int RoomId { get; set; }

            public string MemberId { get; set; }

            public string Name { get; set; }

            public string Tag { get; set; }
        }
    }
}
