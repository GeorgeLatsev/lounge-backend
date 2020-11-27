using Lounge.Services.Users.Models.PrivateRooms;
using Lounge.Services.Users.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Lounge.Services.Users.Models.PrivateRooms.ModelConstants.GroupRoom;

namespace Lounge.Services.Users.Infrastructure.Data.Configurations
{
    public class GroupRoomEntityTypeConfiguration : IEntityTypeConfiguration<GroupRoom>
    {
        public void Configure(EntityTypeBuilder<GroupRoom> builder)
        {
            builder.Property(dm => dm.Name)
                .IsRequired(false)
                .HasMaxLength(MaxNameLength);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(dm => dm.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
