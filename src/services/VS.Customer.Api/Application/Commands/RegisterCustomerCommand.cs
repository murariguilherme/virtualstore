using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Core.DomainObjects;
using VS.Core.Messages;

namespace VS.Customer.Api.Application.Commands
{
    public class RegisterCustomerCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        
        public RegisterCustomerCommand(Guid id, string name, string email)
        {
            this.AggregateId = id;
            this.Id = id;
            this.Name = name;
            this.Email = email;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterCustomerCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RegisterCustomerCommandValidation: AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerCommandValidation()
        {
            RuleFor(r => r.Id)
                .NotEmpty();

            RuleFor(r => r.Name)
                .Length(1, 200)
                .NotEmpty();

            RuleFor(r => r.Email)                
                .Must(CheckEmailIsValid)
                .WithMessage("E-mail is not valid.");
        }

        private bool CheckEmailIsValid(string email)
        {
            return Email.Validate(email);
        }
    }
}
