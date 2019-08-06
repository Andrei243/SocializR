using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class ErrorLogCustomConfiguration : IEntityTypeConfiguration<ErrorLogCustom>
    {
        void IEntityTypeConfiguration<ErrorLogCustom>.Configure(EntityTypeBuilder<ErrorLogCustom> builder)
        {

                builder.Property(e => e.Id).HasColumnName("id");

                builder.Property(e => e.Line).HasColumnName("line");

                builder.Property(e => e.Message)
                    .HasColumnName("message")
                    .IsUnicode(false);

                builder.Property(e => e.Number).HasColumnName("number");

                builder.Property(e => e.Proced)
                    .HasColumnName("proced")
                    .IsUnicode(false);

                builder.Property(e => e.Severity).HasColumnName("severity");

                builder.Property(e => e.State).HasColumnName("state");
        }
    }
}
