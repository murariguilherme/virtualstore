using EasyNetQ;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VS.Core.Mediator;
using VS.Core.Messages.Integration;
using VS.Customer.Api.Application.Commands;

namespace VS.Customer.Api.Services
{
    public class UserRegisteredIntegrationHandler : BackgroundService
    {
        private IBus _bus;
        private IServiceProvider _serviceProvider;

        public UserRegisteredIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus = RabbitHutch.CreateBus("host=localhost");                       
            _bus.RespondAsync<UserRegisteredIntegrationEvent, ResponseMessage>(async request =>
                new ResponseMessage(await RegisterCustomer(request))
            );
            
            return Task.CompletedTask;
        }

        private async Task<ValidationResult> RegisterCustomer(UserRegisteredIntegrationEvent userEvent)
        {
            var command = new RegisterCustomerCommand(userEvent.Id, userEvent.Name, userEvent.Email);
            ValidationResult response;
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                response = await mediator.SendCommand(command);
            }
            
            return response;
        }
    }
}