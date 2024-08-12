using Microsoft.IO;
using Serilog;
using ILogger = Serilog.ILogger;

namespace CommerceHub.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = await FormatRequest(context.Request);
            _logger.Information("Incoming Request: {Request}", request);

            var originalBodyStream = context.Response.Body;

            try
            {
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);

                    var response = await FormatResponse(context.Response);
                    _logger.Information("Outgoing Response: {Response}", response);

                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var bodyAsText = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Position = 0;

            return $"Scheme: {request.Scheme}, Host: {request.Host}, Path: {request.Path}, QueryString: {request.QueryString}, Body: {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"StatusCode: {response.StatusCode}, Body: {text}";
        }
    }
}
