using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace VS.WebApi.Core.User
{
    public class User : IUser
    {
        private readonly IHttpContextAccessor _acessor;

        public User(IHttpContextAccessor acessor)
        {
            _acessor = acessor;
        }

        public string Name { get => _acessor.HttpContext.User.Identity.Name; }

        public IEnumerable<Claim> GetClaims()
        {
            return _acessor.HttpContext.User.Claims;
        }

        public HttpContext GetHttpContext()
        {
            return _acessor.HttpContext;
        }

        public string GetUserEmail()
        {
            return _acessor.HttpContext.User.GetUserEmail();
        }

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(_acessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string GetUserToken()
        {
            return IsAuthenticated() ? _acessor.HttpContext.User.GetUserToken(): "";
        }

        public bool HasRole(string role)
        {
            return _acessor.HttpContext.User.IsInRole(role);
        }

        public bool IsAuthenticated()
        {
            return _acessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
