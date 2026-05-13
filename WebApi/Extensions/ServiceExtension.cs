using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Service;
using WebApi.ExceptionHandlers;

namespace WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void AddService(this IServiceCollection service)
        {
            service.AddFluentValidationAutoValidation();
            service.AddAutoMapper(cfg => { }, typeof(ServiceAssembly).Assembly);
            service.Configure<ApiBehaviorOptions>(x =>
            {
                x.SuppressModelStateInvalidFilter = true;
            });
            service.AddArticleService();
            service.AddExceptionHandler<GlobalExceptionHandler>();
            service.AddProblemDetails();
        }
    }
}
