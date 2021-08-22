using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MISA.AMIS.API.MiddleWare
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //HttpStatusCode status;
            string message;
            var response = new
            {
                devMsg = exception.Message,
                userMsg = /*Properties.Resources.User_msg*/ "",
                MISACode = /*Properties.Resources.MISACode*/ "",
                Data = exception.Data
            };
            var stackTrace = String.Empty;
            message = exception.Message;
            var exceptionType = exception.GetType();
            var result = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            return context.Response.WriteAsync(result);
        }
    }
}
