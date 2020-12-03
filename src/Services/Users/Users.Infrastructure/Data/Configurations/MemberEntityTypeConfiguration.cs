using Lounge.Services.Users.Models.RoomEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lounge.Services.Users.Infrastructure.Data.Configurations
{
    public class MemberEntityTypeConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("RoomMembers");

            builder.HasKey(m => new { m.RoomId, m.UserId });
        }
    }
}
