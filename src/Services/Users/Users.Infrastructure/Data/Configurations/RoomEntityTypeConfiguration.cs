using Lounge.Services.Users.Models.RoomEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lounge.Services.Users.Infrastructure.Data.Configurations
{
    public class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Rooms");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .UseHiLo("rseq");

            builder.HasDiscriminator(r => r.Type)
                .HasValue<Room>(RoomType.Private)
                .HasValue<GroupRoom>(RoomType.Group);

            builder.HasMany(r => r.Members)
                .WithOne(m => m.Room)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
