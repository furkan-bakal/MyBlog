using Microsoft.EntityFrameworkCore;
using Repository;

namespace WebApi.Extensions
{
    public static class RepositoryExtension
    {
        public static void AddRepository(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"));
            });

            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
