using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Core.Data;
using VS.Customer.Api.Models;

namespace VS.Customer.Api.Data
{
    public class CustomerDbContext: DbContext, IUnitOfWork
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options): base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Customer.Api.Models.Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public async Task<bool> CommitAsync()
        {
            return await this.SaveChangesAsync() > 0;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            builder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        }
    }
}
