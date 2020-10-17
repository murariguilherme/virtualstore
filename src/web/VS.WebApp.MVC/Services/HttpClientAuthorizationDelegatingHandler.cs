using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VS.WebApp.MVC.Extensions;
using System.Net.Http.Headers;
using VS.WebApi.Core.User;

namespace VS.WebApp.MVC.Services
{
    public class HttpClientAuthorizationDelegatingHandler: DelegatingHandler
    {
        private readonly IUser _user;
        public HttpClientAuthorizationDelegatingHandler(IUser user)
        {
            _user = user;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authHeader = _user.GetHttpContext().Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authHeader))
            {
                request.Headers.Add("Authorization", new List<string>() { authHeader });
            }

            var token = _user.GetUserToken();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
