using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class InterestsUsersConfiguration : IEntityTypeConfiguration<InterestsUsers>
    {
        void IEntityTypeConfiguration<InterestsUsers>.Configure(EntityTypeBuilder<InterestsUsers> builder)
        {
                builder.HasKey(e => new { e.InterestId, e.UserId })
                    .HasName("INTEREST_LINK_PK");

                builder.HasOne(d => d.Interest)
                    .WithMany(p => p.InterestsUsers)
                    .HasForeignKey(d => d.InterestId)
                    .HasConstraintName("INTEREST_LINK_INTERES_FK");

                builder.HasOne(d => d.User)
                    .WithMany(p => p.InterestsUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("INTEREST_LINK_User_FK");
        }
    }
}
