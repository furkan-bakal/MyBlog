using Microsoft.Extensions.DependencyInjection;

namespace Service
{
    public static class ArticleLikeServiceExtension
    {
        public static void AddArticleLikeService(this IServiceCollection services)
        {
            services.AddScoped<IArticleLikeService, ArticleLikeService>();
        }
    }
}
