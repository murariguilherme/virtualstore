using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Core.Data;
using VS.Core.DomainObjects;
using VS.Core.Mediator;
using VS.Customer.Api.Models;

namespace VS.Customer.Api.Data
{
    public class CustomerDbContext: DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediator;
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options, IMediatorHandler mediator): base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            _mediator = mediator;
        }

        public DbSet<Customer.Api.Models.Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            builder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        }
        public async Task<bool> CommitAsync()
        {
            var success = await this.SaveChangesAsync() > 0;

            if (success)
            {
                await _mediator.PublishEvents(this);
            }

            return success;
        }        
    }

    public static class MediatorExtension
    {
        public static Task PublishEvents<T>(this IMediatorHandler mediatorHandler, T context) where T: DbContext
        {
            var entities = context.ChangeTracker.Entries<Entity>().Where(e => e.Entity.Events != null && e.Entity.Events.Any());

            var events = entities.SelectMany(e => e.Entity.Events).ToList();

            entities.ToList().ForEach(e => e.Entity.ClearEventList());

            var tasks = events.Select(async (eventObj) => { 
                await mediatorHandler.PublishEvent(eventObj); 
            });

            return Task.WhenAll(tasks);
        }
    }
}
