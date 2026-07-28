using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace Service
{
    public static class ArticleCommentServiceExtension
    {
        public static void AddArticleCommentService(this IServiceCollection services)
        {
            services.AddScoped<IArticleCommentService, ArticleCommentService>();
            services.AddScoped<IArticleCommentRepository, ArticleCommentRepository>();
        }
    }
}
