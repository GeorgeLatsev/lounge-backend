using Lounge.Services.Users.Models.RoomEntities;

namespace Lounge.Services.Users.Services.Rooms.Models
{
    public class MemberModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public static MemberModel MapFrom(Member member)
        {
            return new MemberModel
            {
                Id = member.UserId,
                Name = member.User.Name,
                Tag = member.User.Tag
            };
        }
    }
}
