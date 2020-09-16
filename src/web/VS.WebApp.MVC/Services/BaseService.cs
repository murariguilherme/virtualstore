using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VS.WebApp.MVC.Extensions;

namespace VS.WebApp.MVC.Services
{
    public abstract class BaseService
    {
        public bool CanResolveErrorMessages(HttpResponseMessage message)
        {
            switch ((int)message.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(message.StatusCode);
                case 400:
                    return false;
            }

            message.EnsureSuccessStatusCode();
            return true;
        }
    }
}
