using Microsoft.Extensions.DependencyInjection;
using Repository.Category;

namespace Service.Category
{
    public static class CategoryServiceExtension
    {
        public static void AddCategoryService(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }
    }
}
