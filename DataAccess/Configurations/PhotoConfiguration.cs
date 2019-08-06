using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        void IEntityTypeConfiguration<Photo>.Configure(EntityTypeBuilder<Photo> builder)
        {
                builder.Property(e => e.Binar)
                    .IsRequired()
                    .HasColumnType("image");

                builder.HasOne(d => d.Album)
                    .WithMany(p => p.Photo)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("PHOTO_ALBUM_FK");

                builder.HasOne(d => d.Post)
                    .WithMany(p => p.Photo)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("PHOTO_POST_FK");
        }
    }
}
