using Core;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.ExceptionHandlers
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            //logger.LogError(exception, exception.Message);

            var responseModel = ResponseModelDto<NoContent>.Failure(exception.Message, System.Net.HttpStatusCode.InternalServerError);

            await httpContext.Response.WriteAsJsonAsync(responseModel, cancellationToken);

            return true;
        }
    }
}
