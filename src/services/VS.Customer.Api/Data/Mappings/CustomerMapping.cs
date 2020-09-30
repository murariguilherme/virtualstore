

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VS.Core.DomainObjects;

namespace VS.Customer.Api.Data.Mappings
{
    public class CustomerMapping : IEntityTypeConfiguration<Customer.Api.Models.Customer>
    {
        public void Configure(EntityTypeBuilder<Models.Customer> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType($"varchar({Email.EmailAddressMaxLength})");
            });

            builder.HasOne(c => c.Address)
                .WithOne(a => a.Customer);

            builder.ToTable("Customers");
        }
    }
}
