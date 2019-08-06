using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Domain;
using DataAccess.Configurations;

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

            modelBuilder.ApplyConfiguration(new AlbumConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new CountyConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorLogCustomConfiguration());
            modelBuilder.ApplyConfiguration(new FriendshipConfiguration());
            modelBuilder.ApplyConfiguration(new InterestConfiguration());
            modelBuilder.ApplyConfiguration(new InterestsUsersConfiguration());
            modelBuilder.ApplyConfiguration(new LocalityConfiguration());
            modelBuilder.ApplyConfiguration(new PhotoConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new ReactionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            
        }
    }
}
