using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service;
using Service.ExceptionHandlers;
using Service.Token;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void AddService(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddFluentValidationAutoValidation();
            service.AddValidatorsFromAssembly(typeof(ServiceAssembly).Assembly);
            service.AddAutoMapper(cfg => { }, typeof(ServiceAssembly).Assembly);
            service.Configure<ApiBehaviorOptions>(x =>
            {
                x.SuppressModelStateInvalidFilter = true;
            });
            service.AddArticleService();
            service.AddExceptionHandler<GlobalExceptionHandler>();
            service.AddProblemDetails();

            service.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));
            service.Configure<Clients>(configuration.GetSection("Clients"));
            service.AddScoped<ITokenService, TokenService>();
        }
    }
}
