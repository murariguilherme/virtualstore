using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VS.Identity.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        private ICollection<string> Errors = new List<string>();

        protected ActionResult GenerateReponse(object result = null)
        {
            if (IsValid()) return Ok(result);

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", Errors.ToArray() }
            }));
        }

        protected ActionResult GenerateReponse(ModelStateDictionary modelstate)
        {
            var errors = modelstate.Values.SelectMany(e => e.Errors).ToList();

            foreach (var error in errors)
            {
                AddErrorToList(error.ErrorMessage);
            }

            return GenerateReponse();
        }

        private bool IsValid()
        {
            return (!Errors.Any());
        }

        protected void AddErrorToList(string error)
        {
            Errors.Add(error);
        }

        private void ClearErrorList()
        {
            Errors.Clear();
        }
    }
}
