using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VS.MessageBus;

namespace VS.Identity.Api.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetConnectionString("MessageBus"));
        }
    }
}
