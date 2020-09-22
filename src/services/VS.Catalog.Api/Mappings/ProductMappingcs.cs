using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Catalog.Api.Models;

namespace VS.Catalog.Api.Mappings
{
    public class ProductMappingcs : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder
                .Property(p => p.Description)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder
                .Property(p => p.Image)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.ToTable("Products");
        }
    }
}
