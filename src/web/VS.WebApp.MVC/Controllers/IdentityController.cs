using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VS.WebApp.MVC.Services;
using VS.WebApp.MVC.ViewModels;

namespace VS.WebApp.MVC.Controllers
{
    public class IdentityController : Controller
    {
        private IIdentityAuthenticationService _authenticationService;
        public IdentityController(IIdentityAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Route("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid) return View(userLoginViewModel);

            var response = await _authenticationService.Login(userLoginViewModel);

            if (false) return View(userLoginViewModel);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register(UserRegisterViewModel userRegisterViewModel)
        {
            if (!ModelState.IsValid) return View(userRegisterViewModel);

            var response = await _authenticationService.Register(userRegisterViewModel);

            if (false) return View(userRegisterViewModel);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
