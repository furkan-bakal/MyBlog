using Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Service.ExceptionHandlers
{
    public class GlobalExceptionHandler(Service.Logger.IAppLogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var requestPath = httpContext.Request.Path;
            var requestMethod = httpContext.Request.Method;

            logger.Error($"Error occurred while processing the request: {requestMethod} {requestPath}. Message: {exception.Message}", exception);

            var responseModel = ResponseModelDto<NoContent>.Failure(exception.Message, System.Net.HttpStatusCode.InternalServerError);

            await httpContext.Response.WriteAsJsonAsync(responseModel, cancellationToken);

            return true;
        }
    }
}
