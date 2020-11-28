using Lounge.Services.Users.Models.PrivateRooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lounge.Services.Users.Infrastructure.Data.Configurations
{
    public class PrivateRoomEntityTypeConfiguration : IEntityTypeConfiguration<PrivateRoom>
    {
        public void Configure(EntityTypeBuilder<PrivateRoom> builder)
        {
            builder.ToTable("PrivateRooms");

            builder.HasKey(pr => pr.Id);
            builder.Property(pr => pr.Id)
                .UseHiLo("prseq");

            builder.HasDiscriminator(pr => pr.Type)
                .HasValue<PrivateRoom>(PrivateRoomType.Normal)
                .HasValue<GroupRoom>(PrivateRoomType.Group);

            builder.Property(pr => pr.RoomId)
                .IsRequired(false);

            builder.HasMany(pr => pr.Members)
                .WithOne(m => m.Room)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
