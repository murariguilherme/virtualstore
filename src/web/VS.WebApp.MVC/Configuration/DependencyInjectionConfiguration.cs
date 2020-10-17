using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using VS.WebApp.MVC.Extensions;
using VS.WebApp.MVC.Services;
using Polly;
using VS.WebApi.Core.User;

namespace VS.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void ResolveDependencies(this IServiceCollection services)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IIdentityAuthenticationService, IdentityAuthenticationService>();            

            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(RetryPolicy.RetryAndWaitPolicy())                
                .AddTransientHttpErrorPolicy(options =>                
                    options.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, User>();
        }
    }
}
