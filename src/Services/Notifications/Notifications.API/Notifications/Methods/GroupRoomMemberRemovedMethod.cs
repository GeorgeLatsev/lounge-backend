namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public class GroupRoomMemberRemovedMethod : BaseMethod
    {
        private GroupRoomMemberRemovedMethod(GroupRoomMemberRemovedMethodPayload data)
            : base(data)
        { }

        public static GroupRoomMemberRemovedMethod WithArgs(int roomId, string memberId)
        {
            var payload = new GroupRoomMemberRemovedMethodPayload
            {
                RoomId = roomId,
                MemberId = memberId
            };

            return new GroupRoomMemberRemovedMethod(payload);
        }


        private class GroupRoomMemberRemovedMethodPayload
        {
            public int RoomId { get; set; }

            public string MemberId { get; set; }
        }
    }
}
