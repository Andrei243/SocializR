using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        void IEntityTypeConfiguration<Post>.Configure(EntityTypeBuilder<Post> builder)
        {
                builder.HasIndex(e => e.AddingMoment)
                    .HasName("Post_AddingMoment");

                builder.Property(e => e.AddingMoment).HasColumnType("datetime");

                builder.Property(e => e.Confidentiality)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                builder.HasOne(d => d.User)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("POST_USER_FK");
        }
    }
}
