using Core;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace WebApi.Extensions
{
    public static class MiddlewareExtension
    {
        public static void AddMiddleware(this WebApplication app)
        {
            app.UseExceptionHandler();
            #region
            //app.UseExceptionHandler(appBuilder => { 
            //    appBuilder.Run(async context =>
            //    {
            //        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            //        context.Response.ContentType = "application/json";
            //        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
            //        if (exceptionHandlerFeature != null)
            //        {
            //            var exception = exceptionHandlerFeature.Error;

            //            var responseModel = ResponseModelDto<NoContent>.Failure(exception.Message,HttpStatusCode.InternalServerError);

            //            await context.Response.WriteAsJsonAsync(responseModel);
            //        }
            //    });

            //});
            #endregion

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // TLS'i reverse proxy sonlandırıyor; konteynerde Kestrel yalnızca HTTP dinliyor.
            // Koşulsuz yönlendirme burada ya "Failed to determine the https port" uyarısı
            // basar ya da proxy arkasında yönlendirme döngüsü kurar.
            if (app.Configuration.GetValue("Https:Enforce", false))
            {
                app.UseHttpsRedirection();
            }

            // Yüklenen article görselleri wwwroot altından servis edilir
            app.UseStaticFiles();

            app.UseRateLimiter();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Konteyner healthcheck'i için. Reverse proxy bu yolu dışarıya açmıyor.
            app.MapHealthChecks("/health");
        }
    }
}
