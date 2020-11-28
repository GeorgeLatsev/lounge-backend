using Lounge.Services.Users.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Lounge.Services.Users.Models.Users.ModelConstants.User;

namespace Lounge.Services.Users.Infrastructure.Data.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired(false)
                .HasMaxLength(MaxNameLength);

            builder.Property(u => u.Tag)
                .IsRequired(false)
                .HasMaxLength(MaxTagLength);

            builder.OwnsOne(u => u.Settings, settingsBuilder => { });

            builder.HasMany(u => u.Connections)
                .WithOne()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.PrivateRooms)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
