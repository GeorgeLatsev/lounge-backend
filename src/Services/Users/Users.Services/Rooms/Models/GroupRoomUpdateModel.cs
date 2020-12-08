namespace Lounge.Services.Users.Services.Rooms.Models
{
    public class GroupRoomUpdateModel
    {
        public string Name { get; set; }

        public string OwnerId { get; set; }

        public static GroupRoomUpdateModel From(GroupRoomModel groupRoom)
        {
            return new GroupRoomUpdateModel
            {
                Name = groupRoom.Name,
                OwnerId = groupRoom.OwnerId
            };
        }
    }
}
