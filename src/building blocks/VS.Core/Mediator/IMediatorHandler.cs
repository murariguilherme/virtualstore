using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VS.Core.Messages;

namespace VS.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T eventObj) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
    }
}
