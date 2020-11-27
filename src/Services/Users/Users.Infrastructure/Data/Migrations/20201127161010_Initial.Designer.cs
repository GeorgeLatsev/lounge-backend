﻿// <auto-generated />

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lounge.Services.Users.Infrastructure.Data.Migrations
{
    [DbContext(typeof(UsersContext))]
    [Migration("20201127161010_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.HasSequence("prseq")
                .IncrementsBy(10);

            modelBuilder.Entity("Lounge.Services.Users.Models.PrivateRooms.Member", b =>
                {
                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RoomId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("PrivateRoomMembers");
                });

            modelBuilder.Entity("Lounge.Services.Users.Models.PrivateRooms.PrivateRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseHiLo("prseq");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PrivateRooms");

                    b.HasDiscriminator<int>("Type").HasValue(1);
                });

            modelBuilder.Entity("Lounge.Services.Users.Models.Users.Connection", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OtherUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Notes")
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<int>("Relationship")
                        .HasColumnType("int");

                    b.HasKey("UserId", "OtherUserId");

                    b.HasIndex("OtherUserId");

                    b.ToTable("UserConnections");
                });

            modelBuilder.Entity("Lounge.Services.Users.Models.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("Tag")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Lounge.Services.Users.Models.PrivateRooms.GroupRoom", b =>
                {
                    b.HasBaseType("Lounge.Services.Users.Models.PrivateRooms.PrivateRoom");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("OwnerId");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Lounge.Services.Users.Models.PrivateRooms.Member", b =>
                {
                    b.HasOne("Lounge.Services.Users.Models.PrivateRooms.PrivateRoom", "Room")
                        .WithMany("Members")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Lounge.Services.Users.Models.Users.User", "User")
                        .WithMany("PrivateRooms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Lounge.Services.Users.Models.Users.Connection", b =>
                {
                    b.HasOne("Lounge.Services.Users.Models.Users.User", "OtherUser")
                        .WithMany()
                        .HasForeignKey("OtherUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Lounge.Services.Users.Models.Users.User", null)
                        .WithMany("Connections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("OtherUser");
                });

            modelBuilder.Entity("Lounge.Services.Users.Models.Users.User", b =>
                {
                    b.OwnsOne("Lounge.Services.Users.Models.Users.Settings", "Settings", b1 =>
                        {
                            b1.Property<string>("UserId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("Theme")
                                .HasColumnType("int");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("Lounge.Services.Users.Models.PrivateRooms.GroupRoom", b =>
                {
                    b.HasOne("Lounge.Services.Users.Models.Users.User", null)
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Lounge.Services.Users.Models.PrivateRooms.PrivateRoom", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("Lounge.Services.Users.Models.Users.User", b =>
                {
                    b.Navigation("Connections");

                    b.Navigation("PrivateRooms");
                });
#pragma warning restore 612, 618
        }
    }
}