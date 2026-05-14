using Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Service.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            var responseModel = ResponseModelDto<NoContent>.Failure(exception.Message, System.Net.HttpStatusCode.InternalServerError);

            await httpContext.Response.WriteAsJsonAsync(responseModel, cancellationToken);

            return true;
        }
    }
}
