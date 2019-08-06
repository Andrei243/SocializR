using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class LocalityConfiguration : IEntityTypeConfiguration<Locality>
    {
        void IEntityTypeConfiguration<Locality>.Configure(EntityTypeBuilder<Locality> builder)
        {
                builder.HasIndex(e => e.Name)
                    .HasName("Localitate_Nume");

                builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                builder.HasOne(d => d.County)
                    .WithMany(p => p.Locality)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("LOCALITY_COUNTY_FK");
        }
    }
}
