using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        void IEntityTypeConfiguration<Comment>.Configure(EntityTypeBuilder<Comment> builder)
        {
            
                builder.HasIndex(e => new { e.PostId, e.AddingMoment })
                    .HasName("Comment_AddingMoment");

                builder.Property(e => e.AddingMoment).HasColumnType("datetime");

                builder.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(3000);

                builder.HasOne(d => d.Post)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("COMMENT_POST_FK");

                builder.HasOne(d => d.User)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("COMMENT_USER_FK")
                    .OnDelete(DeleteBehavior.Restrict);
          
        }
    }
}
