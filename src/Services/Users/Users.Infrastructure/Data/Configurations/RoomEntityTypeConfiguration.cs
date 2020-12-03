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

            builder.HasKey(pr => pr.Id);
            builder.Property(pr => pr.Id)
                .UseHiLo("rseq");

            builder.HasDiscriminator(pr => pr.Type)
                .HasValue<Room>(RoomType.Private)
                .HasValue<GroupRoom>(RoomType.Group);

            builder.Property(pr => pr.RoomId)
                .IsRequired(false);

            builder.HasMany(pr => pr.Members)
                .WithOne(m => m.Room)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
