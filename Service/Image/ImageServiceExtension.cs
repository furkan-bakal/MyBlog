using Microsoft.Extensions.DependencyInjection;

namespace Service
{
    public static class ImageServiceExtension
    {
        public static void AddImageService(this IServiceCollection services)
        {
            services.AddScoped<IImageService, ImageService>();
        }
    }
}
