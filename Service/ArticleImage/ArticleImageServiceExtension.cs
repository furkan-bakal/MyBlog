using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace Service
{
    public static class ArticleImageServiceExtension
    {
        public static void AddArticleImageService(this IServiceCollection services)
        {
            services.AddScoped<IArticleImageService, ArticleImageService>();
            services.AddScoped<IArticleImageRepository, ArticleImageRepository>();
        }
    }
}
