using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Catalog.Api.Data;
using VS.Catalog.Api.Repository;

namespace VS.Catalog.Api.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<CatalogDbContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
