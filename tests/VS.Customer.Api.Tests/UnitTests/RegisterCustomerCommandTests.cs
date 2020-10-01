using System;
using VS.Customer.Api.Application.Commands;
using Xunit;
using System.Linq;

namespace VS.Customer.Api.Tests.UnitTests
{
    public class RegisterCustomerCommandTests
    {
        [Fact]
        public void RegisterCustomerCommandTests_ExecuteCommand_MustBeInvalid()
        {
            var command = new RegisterCustomerCommand(Guid.Empty, "", "");
            
            Assert.False(command.IsValid());
            Assert.Equal(5, command.ValidationResult.Errors.Count);

            Assert.Contains("'Id' must not be empty.",
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains("'Name' must be between 1 and 200 characters. You entered 0 characters.",
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains("'Name' must not be empty.",
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains("'Email' must be between 5 and 254 characters. You entered 0 characters.",
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains("'Email' must not be empty.",
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));               
        }

        [Fact]
        public void RegisterCustomerCommandTests_ExecuteCommand_MustBeValid()
        {
            var command = new RegisterCustomerCommand(Guid.NewGuid(), "Joseph", "joseph@hotmail.com");

            Assert.True(command.IsValid());           
        }
    }
}
