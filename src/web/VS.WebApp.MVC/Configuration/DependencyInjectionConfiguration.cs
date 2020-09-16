using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.WebApp.MVC.Extensions;
using VS.WebApp.MVC.Services;

namespace VS.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void ResolveDependencies(this IServiceCollection services)
        {
            services.AddHttpClient<IIdentityAuthenticationService, IdentityAuthenticationService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, User>();
        }
    }
}
