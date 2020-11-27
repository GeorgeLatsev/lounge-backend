using Lounge.Services.Users.Core.Models.PrivateRooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lounge.Services.Users.Infrastructure.Data.Configurations
{
    public class MemberEntityTypeConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("PrivateRoomMembers");

            builder.HasKey(m => new { m.RoomId, m.UserId });
        }
    }
}
