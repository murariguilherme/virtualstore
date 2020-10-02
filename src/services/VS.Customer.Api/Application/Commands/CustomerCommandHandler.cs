using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VS.Core.Mediator;
using VS.Core.Messages;
using VS.Customer.Api.Models;

namespace VS.Customer.Api.Application.Commands
{
    public class CustomerCommandHandler: CommandHandler, IRequestHandler<RegisterCustomerCommand, ValidationResult>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICustomerRepository _repository;

        public CustomerCommandHandler(IMediatorHandler mediatorHandler, ICustomerRepository repository)
        {
            _mediatorHandler = mediatorHandler;
            _repository = repository;
        }

        public async Task<ValidationResult> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var customerExistis = await _repository.GetCustomerByEmail(request.Email);

            if (customerExistis != null)
            {
                AddValidationError("Already exists a customer with the given e-mail.");
                return validationResult;
            }

            var customer = new Models.Customer(request.Id, request.Name, request.Email);

            await _repository.AddCustomer(customer);            

            return await DataPersistence(_repository.UnitOfWork);
        }
    }
}
