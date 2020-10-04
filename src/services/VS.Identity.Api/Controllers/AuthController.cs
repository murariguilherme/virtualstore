using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VS.Core.Controllers;
using VS.Core.Messages.Integration;
using VS.Identity.Api.ViewModels;
using VS.WebApi.Core.Identity;

namespace VS.Identity.Api.Controllers
{    
    [Route("api/identity")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private IBus _bus;
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appsettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appsettings.Value;            
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterViewModel userRegisterViewModel)
        {
            if (!ModelState.IsValid) return GenerateResponse(ModelState);

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

            if (!request.Succeeded) return GenerateResponse();

            var registerCustomerResult = await RegisterCustomer(userRegisterViewModel);

            if (!registerCustomerResult.Validation.IsValid) return GenerateResponse(registerCustomerResult.Validation);

            var login = new UserLoginViewModel()
            {
                Email = userRegisterViewModel.Email,
                Password = userRegisterViewModel.Password
            };

            return await Login(login);            
        }

        public async Task<ResponseMessage> RegisterCustomer(UserRegisterViewModel userRegisterViewModel)
        {
            var user = await _userManager.FindByEmailAsync(userRegisterViewModel.Email);
            var integrationEvent = new UserRegisteredIntegrationEvent(Guid.Parse(user.Id), userRegisterViewModel.Name, userRegisterViewModel.Email);
            _bus = RabbitHutch.CreateBus("host=localhost");
            var result = await _bus.RequestAsync<UserRegisteredIntegrationEvent, ResponseMessage>(integrationEvent);
            
            return result;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid) return GenerateResponse(ModelState);

            var response = await _signInManager.PasswordSignInAsync(userLoginViewModel.Email, userLoginViewModel.Password, false, false);            

            if (response.Succeeded) return GenerateResponse(await GenerateJwt(userLoginViewModel.Email));

            AddErrorLoginFailure(response);
            return GenerateResponse();
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

        private async Task<ClaimsIdentity> GetClaimsIdentity(IList<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.Now).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string GenerateTokenEncoded(ClaimsIdentity identityClaims)
        {
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

            return tokenJwtHandler.WriteToken(token);
        }

        private UserLoginResponse GenerateResponseToken(IdentityUser user, string encodedToken, IList<Claim> userClaims)
        {
            return new UserLoginResponse()
            {
                AccessToken = encodedToken,
                ExpiresIn = (int)TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserToken()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserClaims = userClaims.Select(c => new UserClaim() { Type = c.Type, Value = c.Value })
                }
            };
        }

        private async Task<UserLoginResponse> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userClaims = await _userManager.GetClaimsAsync(user);            
            var identityClaims = await GetClaimsIdentity(userClaims, user);

            var encodedtoken = GenerateTokenEncoded(identityClaims);
            
            return GenerateResponseToken(user, encodedtoken, userClaims);
        }

        private static long ToUnixEpochDate(DateTime date) 
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
