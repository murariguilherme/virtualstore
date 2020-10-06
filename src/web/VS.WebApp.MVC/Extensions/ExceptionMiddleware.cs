using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace VS.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomHttpRequestException ex)
            {
                HandleRequestExceptionAsync(context, ex);
            }
        }

        private static void HandleRequestExceptionAsync(HttpContext context, CustomHttpRequestException ex)
        {
            if (ex._httpStatusCode == HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect("/login");                
                return;
            }

            context.Response.StatusCode = (int)ex._httpStatusCode;
        }
    }
}
