using Core;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Identity;
using Service;
using Service.ExceptionHandlers;
using Service.Logger;
using Service.Token;
using Service.User;
using System.Text;

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

            // Logger DI kaydı
            service.AddSingleton(typeof(IAppLogger<>), typeof(SerilogAppLogger<>));

            service.Configure<CustomTokenOptions>(configuration.GetSection("TokenOptions"));
            service.Configure<Clients>(configuration.GetSection("Clients"));
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<UserService>();
            service.AddValidatorsFromAssemblyContaining<CoreAssembly>();
            service.AddIdentityExt();
            service.AddAuthenticationExt(configuration);
        }

        public static async Task seedUserData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            await UserSeedData.SeedAsync(userManager, roleManager);
        }

        public static void AddIdentityExt(this IServiceCollection service)
        {
            service.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<AppDbContext>();
        }

        public static void AddAuthenticationExt(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var tokenOptions = configuration.GetSection("TokenOptions").Get<CustomTokenOptions>()!;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = tokenOptions.Issuer,
                    ValidateIssuer = true,

                    ValidAudience = tokenOptions.Audience,
                    ValidateAudience = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Signature)),

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        }
    }
}