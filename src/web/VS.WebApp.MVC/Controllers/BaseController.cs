using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VS.WebApp.MVC.ViewModels;

namespace VS.WebApp.MVC.Controllers
{
    public abstract class BaseController : Controller
    {
        public bool HasErrors(ResponseResult responseResult)
        {
            if (responseResult != null && responseResult.Errors.Messages.Any())
            {
                foreach (var error in responseResult.Errors.Messages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                
                return true;
            }

            return false;
        }
    }
}
