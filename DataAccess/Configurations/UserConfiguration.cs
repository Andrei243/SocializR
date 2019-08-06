using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<Users>
    {
        void IEntityTypeConfiguration<Users>.Configure(EntityTypeBuilder<Users> builder)
        {
                builder.HasIndex(e => e.Email)
                    .HasName("User_Email");

                builder.HasIndex(e => new { e.Name, e.Surname })
                    .HasName("User_Search");

                builder.Property(e => e.BirthDay).HasColumnType("date");

                builder.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                builder.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                builder.Property(e => e.SexualIdentity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                builder.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                builder.Property(e => e.Vizibility)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                builder.HasOne(e => e.Photo)
                    .WithOne(u => u.Users)
                    .HasForeignKey<Users>(e => e.PhotoId)
                    .IsRequired(false)
                    .HasConstraintName("USER_PROFILE_PHOTO_FK");

                builder.HasOne(d => d.Locality)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.LocalityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("USER_LOCALITY_FK");

                builder.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USER_ROLE_FK");
        }
    }
}
