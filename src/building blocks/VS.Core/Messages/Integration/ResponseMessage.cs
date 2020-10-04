using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace VS.Core.Messages.Integration
{
    public class ResponseMessage: Message
    {
        public ValidationResult Validation { get; private set; }

        public ResponseMessage(ValidationResult validation)
        {
            Validation = validation;
        }
    }
}
