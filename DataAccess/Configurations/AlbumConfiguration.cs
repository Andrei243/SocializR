using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace DataAccess.Configurations
{
    internal class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        void IEntityTypeConfiguration<Album>.Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Album> builder)
        {
          
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasOne(d => d.User)
                .WithMany(p => p.Album)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("ALBUM_USER_FK");
            


        }
    }
}
