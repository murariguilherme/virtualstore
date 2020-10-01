using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VS.Core.Mediator;
using VS.Core.Messages;

namespace VS.Customer.Api.Application.Commands
{
    public class CustomerCommandHandler: CommandHandler, IRequestHandler<RegisterCustomerCommand, ValidationResult>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CustomerCommandHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task<ValidationResult> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            return request.ValidationResult;
        }
    }
}
