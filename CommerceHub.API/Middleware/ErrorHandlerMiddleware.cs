using CommerceHub.Base.Exception;
using CommerceHub.Bussiness.Exceptions;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace CommerceHub.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
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
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode;
            string message;

            switch (ex)
            {
                case Bussiness.Exceptions.ValidationException validationException:
                    statusCode = validationException.StatusCode;
                    message = validationException.ToString();
                    break;
                case CustomException _:
                    statusCode = ((CustomException)ex).StatusCode;
                    message = ex.Message;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            Log.Fatal(
                $"Path={context.Request.Path} || " +
                $"Method={context.Request.Method} || " +
                $"Exception={ex.Message}"
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(
                JsonSerializer.Serialize(ApiResponse<object>.ErrorResult(message)));

        }
    }
}
