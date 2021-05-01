using Lounge.Services.Users.Models.RoomEntities;
using System.Collections.Generic;
using System.Linq;

namespace Lounge.Services.Users.Services.Rooms.Models
{
    public class RoomModel
    {
        public int Id { get; set; }

        public RoomType Type { get; set; }

        public IEnumerable<MemberModel> Members { get; set; }

        public static RoomModel MapFrom(Room room)
        {
            if (room.Type == RoomType.Group)
            {
                var groupRoom = room as GroupRoom;

                return new GroupRoomModel
                {
                    Id = groupRoom.Id,
                    Type = groupRoom.Type,
                    Name = groupRoom.Name,
                    OwnerId = groupRoom.OwnerId,
                    Members = groupRoom.Members.Select(MemberModel.MapFrom)
                };
            }

            return new RoomModel
            {
                Id = room.Id,
                Type = room.Type,
                Members = room.Members.Select(MemberModel.MapFrom)
            };
        }
    }
}
