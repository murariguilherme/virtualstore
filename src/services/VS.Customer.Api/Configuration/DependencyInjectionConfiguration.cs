using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VS.Core.Mediator;
using VS.Customer.Api.Application.Commands;

namespace VS.Customer.Api.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, CustomerCommandHandler>();
        }
    }
}
