using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VS.Core.Data;

namespace VS.Core.Messages
{
    public class CommandHandler
    {
        protected ValidationResult validationResult;

        public CommandHandler()
        {
            validationResult = new ValidationResult();
        }

        public void AddValidationError(string errorMessage)
        {
            validationResult.Errors.Add(new ValidationFailure(string.Empty, errorMessage));
        }

        public async Task<ValidationResult> DataPersistence(IUnitOfWork unitOfWork)
        {
            if (!await unitOfWork.CommitAsync()) this.AddValidationError("An error ocurred on data persistence");                
          
            return validationResult;
        }
    }
}
