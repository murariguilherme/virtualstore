using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VS.Core.Messages
{
    public abstract class Command: Message, IRequest<ValidationResult>
    {
        public DateTime TimeStamp { get; set; }
        public ValidationResult ValidationResult { get; set; }
        protected Command()
        {
            this.TimeStamp = DateTime.Now;
        }

        public abstract bool IsValid();
    }
}
