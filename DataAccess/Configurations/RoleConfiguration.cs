using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        void IEntityTypeConfiguration<Role>.Configure(EntityTypeBuilder<Role> builder)
        {
                builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
        }
    }
}
