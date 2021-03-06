﻿using Lounge.Services.Users.Models.ConnectionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Lounge.Services.Users.Models.ConnectionEntities.ModelConstants.Connection;

namespace Lounge.Services.Users.Infrastructure.Data.Configurations
{
    class ConnectionEntityTypeConfiguration : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder.ToTable("UserConnections");

            builder.HasKey(c => new { c.UserId, OtherUserId = c.OtherId });

            builder.HasOne(c => c.OtherUser)
                .WithMany()
                .HasForeignKey(c => c.OtherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Notes)
                .HasMaxLength(MaxNotesLength);
        }
    }
}
