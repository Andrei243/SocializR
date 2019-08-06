using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        void IEntityTypeConfiguration<Reaction>.Configure(EntityTypeBuilder<Reaction> builder)
        {
                builder.HasKey(e => new { e.PostId, e.UserId })
                    .HasName("REACTION_PK");

                builder.HasOne(d => d.Post)
                    .WithMany(p => p.Reaction)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("REACTION_POSTARE_FK");

                builder.HasOne(d => d.User)
                    .WithMany(p => p.Reaction)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("REACTION_USER_FK");
        }
    }
}
