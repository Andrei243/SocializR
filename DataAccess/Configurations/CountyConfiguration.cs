using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class CountyConfiguration : IEntityTypeConfiguration<County>
    {
        void IEntityTypeConfiguration<County>.Configure(EntityTypeBuilder<County> builder)
        {
           
                builder.HasIndex(e => e.Name)
                    .HasName("County_Name");

                builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
           
        }
    }
}
