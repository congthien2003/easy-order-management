using StoreManagement.Application.Common;
using System.Net;

namespace StoreManagement.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception (optional)
            _logger.LogError(exception, "An unhandled exception occurred.");
            var statusCode = HttpStatusCode.InternalServerError;
            string result;
            switch (exception)
            {
                case KeyNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    result = exception.Message;
                    break;

                case NullReferenceException _:
                    statusCode = HttpStatusCode.BadRequest;
                    result = exception.Message;
                    break;

                default:
                    result = "An unexpected error occurred.";
                    break;
            }
            // Set the status code and return the error response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsJsonAsync(Result.Failure(exception.Message));
        }
    }
}
