using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Domain;

namespace DataAccess
{
    public partial class SocializRContext : DbContext
    {
        public SocializRContext()
        {
        }

        public SocializRContext(DbContextOptions<SocializRContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<ErrorLogCustom> ErrorLogCustom { get; set; }
        public virtual DbSet<Friendship> Friendship { get; set; }
        public virtual DbSet<Interest> Interest { get; set; }
        public virtual DbSet<InterestsUsers> InterestsUsers { get; set; }
        public virtual DbSet<Locality> Locality { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Reaction> Reaction { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=ANITU;Database=SocializR;Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Album)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("ALBUM_USER_FK");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasIndex(e => new { e.PostId, e.AddingMoment })
                    .HasName("Comment_AddingMoment");

                entity.Property(e => e.AddingMoment).HasColumnType("datetime");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(3000);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("COMMENT_POST_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("COMMENT_USER_FK");
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("County_Name");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ErrorLogCustom>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Line).HasColumnName("line");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .IsUnicode(false);

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.Proced)
                    .HasColumnName("proced")
                    .IsUnicode(false);

                entity.Property(e => e.Severity).HasColumnName("severity");

                entity.Property(e => e.State).HasColumnName("state");
            });

            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.HasKey(e => new { e.IdSender, e.IdReceiver })
                    .HasName("FRIENDSHIP_PK");

                entity.HasOne(d => d.IdReceiverNavigation)
                    .WithMany(p => p.FriendshipIdReceiverNavigation)
                    .HasForeignKey(d => d.IdReceiver)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FRIENDSHIP_USER_RECEIVER_FK");

                entity.HasOne(d => d.IdSenderNavigation)
                    .WithMany(p => p.FriendshipIdSenderNavigation)
                    .HasForeignKey(d => d.IdSender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FRIENDSHIP_USER_SENDER_FK");
            });

            modelBuilder.Entity<Interest>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Interest_Denumire");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InterestsUsers>(entity =>
            {
                entity.HasKey(e => new { e.InterestId, e.UserId })
                    .HasName("INTEREST_LINK_PK");

                entity.HasOne(d => d.Interest)
                    .WithMany(p => p.InterestsUsers)
                    .HasForeignKey(d => d.InterestId)
                    .HasConstraintName("INTEREST_LINK_INTERES_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InterestsUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("INTEREST_LINK_User_FK");
            });

            modelBuilder.Entity<Locality>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Localitate_Nume");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.County)
                    .WithMany(p => p.Locality)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("LOCALITY_COUNTY_FK");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.Binar)
                    .IsRequired()
                    .HasColumnType("image");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Photo)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("PHOTO_ALBUM_FK");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Photo)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("PHOTO_POST_FK");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasIndex(e => e.AddingMoment)
                    .HasName("Post_AddingMoment");

                entity.Property(e => e.AddingMoment).HasColumnType("datetime");

                entity.Property(e => e.Vizibilitate)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("POST_USER_FK");
            });

            modelBuilder.Entity<Reaction>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.UserId })
                    .HasName("REACTION_PK");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reaction)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("REACTION_POSTARE_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reaction)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("REACTION_USER_FK");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("User_Email");

                entity.HasIndex(e => new { e.Name, e.Surname })
                    .HasName("User_Search");

                entity.Property(e => e.BirthDay).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SexualIdentity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Vizibility)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Locality)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.LocalityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("USER_LOCALITY_FK");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USER_ROLE_FK");
            });
        }
    }
}
