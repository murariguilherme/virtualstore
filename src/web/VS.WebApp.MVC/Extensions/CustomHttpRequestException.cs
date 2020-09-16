using System;
using System.Net;
using System.Net.Http;

namespace VS.WebApp.MVC.Extensions
{
    public class CustomHttpRequestException: Exception
    {
        public HttpStatusCode _httpStatusCode;
        public CustomHttpRequestException(HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }
    }
}
