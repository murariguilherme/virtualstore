using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VS.Identity.Api.Extensions;
using VS.Identity.Api.ViewModels;

namespace VS.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appsettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appsettings.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterViewModel userRegisterViewModel)
        {
            if (!ModelState.IsValid) return GenerateReponse(ModelState);

            var user = new IdentityUser()
            {
                UserName = userRegisterViewModel.Email,
                Email = userRegisterViewModel.Email,
                EmailConfirmed = true
            };
            
            var request = await _userManager.CreateAsync(user, userRegisterViewModel.Password);

            foreach (var error in request.Errors)
            {
                AddErrorToList(error.Description);
            }

            if (!request.Succeeded) return GenerateReponse();            

            var login = new UserLoginViewModel()
            {
                Email = userRegisterViewModel.Email,
                Password = userRegisterViewModel.Password
            };

            return await Login(login);            
        }   

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid) return GenerateReponse(ModelState);

            var response = await _signInManager.PasswordSignInAsync(userLoginViewModel.Email, userLoginViewModel.Password, false, false);            

            if (response.Succeeded) return GenerateReponse(await GenerateJwt(userLoginViewModel.Email));

            AddErrorLoginFailure(response);
            return GenerateReponse();
        }

        private void AddErrorLoginFailure(Microsoft.AspNetCore.Identity.SignInResult response)
        {
            if (response.IsLockedOut)
            {
                AddErrorToList("User is locked out.");
            }

            if (response.IsNotAllowed)
            {
                AddErrorToList("User is not allowed.");
            }

            if (response.RequiresTwoFactor)
            {
                AddErrorToList("User requires two factor authentication.");
            }

            AddErrorToList("E-mail or password incorrect.");
        }

        private async Task<UserLoginResponse> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.Now).ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64));

            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim("type", role));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(userClaims);

            var tokenJwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);

            var token = tokenJwtHandler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedtoken = tokenJwtHandler.WriteToken(token);

            var response = new UserLoginResponse()
            {
                AccessToken = encodedtoken,
                ExpiresIn = (int)TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserToken()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserClaims = userClaims.Select(c => new UserClaim() { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date) 
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
