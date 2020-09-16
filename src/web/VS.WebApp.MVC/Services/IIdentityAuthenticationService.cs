using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.WebApp.MVC.ViewModels;

namespace VS.WebApp.MVC.Services
{
    public interface IIdentityAuthenticationService
    {
        Task<UserResponseToken> Login(UserLoginViewModel userLoginViewModel);
        Task<UserResponseToken> Register(UserRegisterViewModel userRegisterViewModel);
    }
}
