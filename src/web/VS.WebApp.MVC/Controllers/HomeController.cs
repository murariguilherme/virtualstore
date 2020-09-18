using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VS.WebApp.MVC.Models;

namespace VS.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/error/{errorCode:length(3,3)}")]
        public IActionResult Error(int errorCode)
        {
            var errorViewModel = new ErrorViewModel();
            errorViewModel.ErrorCode = errorCode;

            if (errorCode == 500)
            {
                errorViewModel.Title = "Ops!";
                errorViewModel.Message = "An error ocurred, try again later.";
            }

            else if (errorCode == 404)
            {
                errorViewModel.Title = "Ops!";
                errorViewModel.Message = "Not found!";
            }

            else if (errorCode == 403)
            {
                errorViewModel.Title = "Forbidden!";
                errorViewModel.Message = "You're not allowed to access this page.";
            }

            else return StatusCode(404);

            return View("Error", errorViewModel);
        }
    }
}
