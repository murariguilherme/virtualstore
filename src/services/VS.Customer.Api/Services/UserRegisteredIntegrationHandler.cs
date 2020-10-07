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
using VS.MessageBus;

namespace VS.Customer.Api.Services
{
    public class UserRegisteredIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;        
        private readonly IServiceProvider _serviceProvider;

        public UserRegisteredIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            SetResponder();
            return Task.CompletedTask;
        }

        private async Task<ResponseMessage> RegisterCustomer(UserRegisteredIntegrationEvent userEvent)
        {
            var command = new RegisterCustomerCommand(userEvent.Id, userEvent.Name, userEvent.Email);
            ValidationResult response;
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                response = await mediator.SendCommand(command);
            }
            
            return new ResponseMessage(response);
        }

        private void SetResponder()
        {
            _bus.RespondAsync<UserRegisteredIntegrationEvent, ResponseMessage>(async request =>
                await RegisterCustomer(request)
            );
            _bus.AdvancedBus.Connected += OnConnect;
        }

        private void OnConnect(object o, EventArgs args)
        {
            SetResponder();
        }
    }
}