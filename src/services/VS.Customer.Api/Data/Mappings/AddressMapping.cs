using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Customer.Api.Models;

namespace VS.Customer.Api.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
                .HasKey(a => a.Id);

            builder
                .Property(a => a.AddressLine1)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder
                .Property(a => a.AddressLine2)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.ToTable("Addresses");
        }
    }
}
