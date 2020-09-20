using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VS.WebApp.MVC.Extensions;
using VS.WebApp.MVC.ViewModels;

namespace VS.WebApp.MVC.Services
{
    public class IdentityAuthenticationService : BaseService, IIdentityAuthenticationService
    {
        private HttpClient _httpClient;        
        public IdentityAuthenticationService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Value.EndpointAuthentication);
        }
        public async Task<UserResponseToken> Login(UserLoginViewModel userLoginViewModel)
        {
            var loginContent = GetContent(userLoginViewModel);

            var response = await _httpClient.PostAsync("/api/identity/login", loginContent);            
            
            if (!CanResolveErrorMessages(response))
            {
                return new UserResponseToken()
                {
                    ResponseResult = await DeserializeObjectAsync<ResponseResult>(response)                    
                };
            }

            return await DeserializeObjectAsync<UserResponseToken>(response);
        }

        public async Task<UserResponseToken> Register(UserRegisterViewModel userRegisterViewModel)
        {
            var registerContent = GetContent(userRegisterViewModel);

            var response = await _httpClient.PostAsync("/api/identity/register", registerContent);            

            if (!CanResolveErrorMessages(response))
            {
                return new UserResponseToken()
                {
                    ResponseResult = await DeserializeObjectAsync<ResponseResult>(response)
                };
            }
            return await DeserializeObjectAsync<UserResponseToken>(response);            
        }
    }
}
