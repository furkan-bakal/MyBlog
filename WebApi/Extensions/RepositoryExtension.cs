using Microsoft.EntityFrameworkCore;
using Repository;
using System.Reflection;

namespace WebApi.Extensions
{
    public static class RepositoryExtension
    {
        public static void AddRepository(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"), optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(Assembly.GetAssembly(typeof(RepositoryAssembly))!.GetName().Name);
                });
            });

            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
