using Microsoft.EntityFrameworkCore;
using VS.Cart.Api.Models;
using System.Linq;

namespace VS.Cart.Api.Data
{
    public class CartDbContext: DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options): base(options) 
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<CustomerCart> CustomerCarts { get; set; }
        public DbSet<CustomerCartProduct> CustomerCartProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                         p => p.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Entity<CustomerCart>()
                .HasIndex(c => c.CustomerId)
                .HasName("IDX_Customer");

            modelBuilder.Entity<CustomerCart>()
                .HasMany(c => c.CustomerCartProducts)
                .WithOne(i => i.CustomerCart)
                .HasForeignKey(c => c.Id);
        }
    }
}
