using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class InterestConfiguration : IEntityTypeConfiguration<Interest>
    {
        void IEntityTypeConfiguration<Interest>.Configure(EntityTypeBuilder<Interest> builder)
        {
                builder.HasIndex(e => e.Name)
                    .HasName("Interest_Denumire");

                builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
        }
    }
}
