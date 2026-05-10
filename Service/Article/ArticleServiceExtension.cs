using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace Service
{
    public static class ArticleServiceExtension
    {
        public static void AddArticleService(this IServiceCollection services)
        {
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
        }
    }
}
