using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VS.WebApp.MVC.Configuration;

namespace VS.WebApp.MVC
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostingEnvironment.EnvironmentName == "Development")
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }        

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityConfiguration();
            services.AddWebAppConfiguration(Configuration);
            services.ResolveDependencies();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseIdentityConfiguration();
            app.UseWebAppConfiguration(env);
        }
    }
}
