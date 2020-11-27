using Lounge.Services.Users.Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Lounge.Services.Users.Core.Models.Users.ModelConstants.Connection;

namespace Lounge.Services.Users.Infrastructure.Data.Configurations
{
    class ConnectionEntityTypeConfiguration : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder.ToTable("UserConnections");

            builder.HasKey(c => new { c.UserId, c.OtherUserId });

            builder.HasOne(c => c.OtherUser)
                .WithMany()
                .HasForeignKey(c => c.OtherUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Notes)
                .HasMaxLength(MaxNotesLength);
        }
    }
}
