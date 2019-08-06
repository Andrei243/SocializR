﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(SocializRContext))]
    partial class SocializRContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Album");
                });

            modelBuilder.Entity("Domain.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddingMoment")
                        .HasColumnType("datetime");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(3000);

                    b.Property<int>("PostId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("PostId", "AddingMoment")
                        .HasName("Comment_AddingMoment");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Domain.County", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .HasName("County_Name");

                    b.ToTable("County");
                });

            modelBuilder.Entity("Domain.ErrorLogCustom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Line")
                        .HasColumnName("line");

                    b.Property<string>("Message")
                        .HasColumnName("message")
                        .IsUnicode(false);

                    b.Property<int?>("Number")
                        .HasColumnName("number");

                    b.Property<string>("Proced")
                        .HasColumnName("proced")
                        .IsUnicode(false);

                    b.Property<int?>("Severity")
                        .HasColumnName("severity");

                    b.Property<int?>("State")
                        .HasColumnName("state");

                    b.HasKey("Id");

                    b.ToTable("ErrorLogCustom");
                });

            modelBuilder.Entity("Domain.Friendship", b =>
                {
                    b.Property<int>("IdSender");

                    b.Property<int>("IdReceiver");

                    b.Property<bool?>("Accepted");

                    b.Property<DateTime>("CreatedOn");

                    b.HasKey("IdSender", "IdReceiver")
                        .HasName("FRIENDSHIP_PK");

                    b.HasIndex("IdReceiver");

                    b.ToTable("Friendship");
                });

            modelBuilder.Entity("Domain.Interest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .HasName("Interest_Denumire");

                    b.ToTable("Interest");
                });

            modelBuilder.Entity("Domain.InterestsUsers", b =>
                {
                    b.Property<int>("InterestId");

                    b.Property<int>("UserId");

                    b.HasKey("InterestId", "UserId")
                        .HasName("INTEREST_LINK_PK");

                    b.HasIndex("UserId");

                    b.ToTable("InterestsUsers");
                });

            modelBuilder.Entity("Domain.Locality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountyId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("CountyId");

                    b.HasIndex("Name")
                        .HasName("Localitate_Nume");

                    b.ToTable("Locality");
                });

            modelBuilder.Entity("Domain.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AlbumId");

                    b.Property<byte[]>("Binar")
                        .IsRequired()
                        .HasColumnType("image");

                    b.Property<int?>("Position");

                    b.Property<int?>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("PostId");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("Domain.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddingMoment")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId");

                    b.Property<string>("Vizibilitate")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("AddingMoment")
                        .HasName("Post_AddingMoment");

                    b.HasIndex("UserId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("Domain.Reaction", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("UserId");

                    b.HasKey("PostId", "UserId")
                        .HasName("REACTION_PK");

                    b.HasIndex("UserId");

                    b.ToTable("Reaction");
                });

            modelBuilder.Entity("Domain.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Domain.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<int?>("LocalityId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<int?>("PhotoId");

                    b.Property<int>("RoleId");

                    b.Property<string>("SexualIdentity")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("Vizibility")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .HasName("User_Email");

                    b.HasIndex("LocalityId");

                    b.HasIndex("PhotoId")
                        .IsUnique()
                        .HasFilter("[PhotoId] IS NOT NULL");

                    b.HasIndex("RoleId");

                    b.HasIndex("Name", "Surname")
                        .HasName("User_Search");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Album", b =>
                {
                    b.HasOne("Domain.Users", "User")
                        .WithMany("Album")
                        .HasForeignKey("UserId")
                        .HasConstraintName("ALBUM_USER_FK")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Comment", b =>
                {
                    b.HasOne("Domain.Post", "Post")
                        .WithMany("Comment")
                        .HasForeignKey("PostId")
                        .HasConstraintName("COMMENT_POST_FK")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Users", "User")
                        .WithMany("Comment")
                        .HasForeignKey("UserId")
                        .HasConstraintName("COMMENT_USER_FK");
                });

            modelBuilder.Entity("Domain.Friendship", b =>
                {
                    b.HasOne("Domain.Users", "IdReceiverNavigation")
                        .WithMany("FriendshipIdReceiverNavigation")
                        .HasForeignKey("IdReceiver")
                        .HasConstraintName("FRIENDSHIP_USER_RECEIVER_FK");

                    b.HasOne("Domain.Users", "IdSenderNavigation")
                        .WithMany("FriendshipIdSenderNavigation")
                        .HasForeignKey("IdSender")
                        .HasConstraintName("FRIENDSHIP_USER_SENDER_FK");
                });

            modelBuilder.Entity("Domain.InterestsUsers", b =>
                {
                    b.HasOne("Domain.Interest", "Interest")
                        .WithMany("InterestsUsers")
                        .HasForeignKey("InterestId")
                        .HasConstraintName("INTEREST_LINK_INTERES_FK")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Users", "User")
                        .WithMany("InterestsUsers")
                        .HasForeignKey("UserId")
                        .HasConstraintName("INTEREST_LINK_User_FK")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Locality", b =>
                {
                    b.HasOne("Domain.County", "County")
                        .WithMany("Locality")
                        .HasForeignKey("CountyId")
                        .HasConstraintName("LOCALITY_COUNTY_FK")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Photo", b =>
                {
                    b.HasOne("Domain.Album", "Album")
                        .WithMany("Photo")
                        .HasForeignKey("AlbumId")
                        .HasConstraintName("PHOTO_ALBUM_FK");

                    b.HasOne("Domain.Post", "Post")
                        .WithMany("Photo")
                        .HasForeignKey("PostId")
                        .HasConstraintName("PHOTO_POST_FK");
                });

            modelBuilder.Entity("Domain.Post", b =>
                {
                    b.HasOne("Domain.Users", "User")
                        .WithMany("Post")
                        .HasForeignKey("UserId")
                        .HasConstraintName("POST_USER_FK")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Reaction", b =>
                {
                    b.HasOne("Domain.Post", "Post")
                        .WithMany("Reaction")
                        .HasForeignKey("PostId")
                        .HasConstraintName("REACTION_POSTARE_FK")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Users", "User")
                        .WithMany("Reaction")
                        .HasForeignKey("UserId")
                        .HasConstraintName("REACTION_USER_FK");
                });

            modelBuilder.Entity("Domain.Users", b =>
                {
                    b.HasOne("Domain.Locality", "Locality")
                        .WithMany("Users")
                        .HasForeignKey("LocalityId")
                        .HasConstraintName("USER_LOCALITY_FK")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Domain.Photo", "Photo")
                        .WithOne("Users")
                        .HasForeignKey("Domain.Users", "PhotoId")
                        .HasConstraintName("USER_PROFILE_PHOTO_FK");

                    b.HasOne("Domain.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("USER_ROLE_FK");
                });
#pragma warning restore 612, 618
        }
    }
}
