using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        void IEntityTypeConfiguration<Friendship>.Configure(EntityTypeBuilder<Friendship> builder)
        {
                builder.HasKey(e => new { e.IdSender, e.IdReceiver })
                    .HasName("FRIENDSHIP_PK");

                builder.HasOne(d => d.IdReceiverNavigation)
                    .WithMany(p => p.FriendshipIdReceiverNavigation)
                    .HasForeignKey(d => d.IdReceiver)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FRIENDSHIP_USER_RECEIVER_FK");

                builder.HasOne(d => d.IdSenderNavigation)
                    .WithMany(p => p.FriendshipIdSenderNavigation)
                    .HasForeignKey(d => d.IdSender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FRIENDSHIP_USER_SENDER_FK");
        }
    }
}
