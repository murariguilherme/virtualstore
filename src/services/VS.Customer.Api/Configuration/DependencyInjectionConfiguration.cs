using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VS.Core.Mediator;
using VS.Customer.Api.Application.Commands;
using VS.Customer.Api.Application.Events;
using VS.Customer.Api.Data;
using VS.Customer.Api.Models;

namespace VS.Customer.Api.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, CustomerCommandHandler>();
            services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerEventHandler>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
