using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace VS.Core.Messages
{
    public class CommandHandler
    {
        private readonly ValidationResult validationResult;

        public CommandHandler()
        {
            validationResult = new ValidationResult();
        }

        public void AddValidationError(string errorMessage)
        {
            validationResult.Errors.Add(new ValidationFailure(string.Empty, errorMessage));
        }
    }
}
