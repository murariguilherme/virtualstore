using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using VS.WebApp.MVC.Services;
using VS.WebApp.MVC.ViewModels;

namespace VS.WebApp.MVC.Controllers
{
    public class IdentityController : BaseController
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

            if (HasErrors(response.ResponseResult)) return View(userLoginViewModel);

            await AddLoginInformationToCookie(response);

            return RedirectToAction("Index", "Catalog");
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

            if (HasErrors(response.ResponseResult)) return View(userRegisterViewModel);

            await AddLoginInformationToCookie(response);

            return RedirectToAction("Index", "");
        }

        [HttpGet]
        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task AddLoginInformationToCookie(UserResponseToken userResponseToken)
        {
            var token = GetFormatToken(userResponseToken.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", userResponseToken.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties() { ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60), IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return;
        }

        public static JwtSecurityToken GetFormatToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        }
    }
}
