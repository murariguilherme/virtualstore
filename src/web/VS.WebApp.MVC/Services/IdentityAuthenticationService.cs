using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VS.WebApp.MVC.ViewModels;

namespace VS.WebApp.MVC.Services
{
    public class IdentityAuthenticationService : BaseService, IIdentityAuthenticationService
    {
        private HttpClient _httpClient;

        public IdentityAuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UserResponseToken> Login(UserLoginViewModel userLoginViewModel)
        {
            var loginContent = new StringContent
                (
                    JsonSerializer.Serialize(userLoginViewModel), Encoding.UTF8, "application/json"
                );                

            var response = await _httpClient.PostAsync("https://localhost:44384/api/identity/login", loginContent);            
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            if (!CanResolveErrorMessages(response))
            {
                return new UserResponseToken()
                {
                    ResponseResult = await JsonSerializer.DeserializeAsync<ResponseResult>(await response.Content.ReadAsStreamAsync(), options)
                };
            }

            return await JsonSerializer.DeserializeAsync<UserResponseToken>(await response.Content.ReadAsStreamAsync(), options);
        }

        public async Task<UserResponseToken> Register(UserRegisterViewModel userRegisterViewModel)
        {
            var registerContent = new StringContent
                (
                    JsonSerializer.Serialize(userRegisterViewModel), Encoding.UTF8, "application/json"
                );

            var response = await _httpClient.PostAsync("https://localhost:44384/api/identity/register", registerContent);
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            if (!CanResolveErrorMessages(response))
            {
                return new UserResponseToken()
                {
                    ResponseResult = await JsonSerializer.DeserializeAsync<ResponseResult>(await response.Content.ReadAsStreamAsync(), options)
                };
            }

            return await JsonSerializer.DeserializeAsync<UserResponseToken>(await response.Content.ReadAsStreamAsync());
        }
    }
}
