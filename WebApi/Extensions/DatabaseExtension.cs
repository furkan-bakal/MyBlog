using Microsoft.EntityFrameworkCore;
using Repository;
using Serilog;

namespace WebApi.Extensions
{
    public static class DatabaseExtension
    {
        /// <summary>
        /// Bekleyen EF migration'larını uygular, veritabanı henüz ayağa kalkmadıysa tekrar dener.
        /// Compose'daki healthcheck çoğu durumu zaten çözer; ancak "soket açık ama initdb sürüyor"
        /// aralığı ve compose dışı yeniden başlatmalar için tekrar denemek gerekiyor.
        /// </summary>
        public static async Task MigrateDatabaseAsync(this WebApplication app)
        {
            if (!app.Configuration.GetValue("Database:AutoMigrate", true))
            {
                Log.Information("Database:AutoMigrate kapalı, migration atlanıyor.");
                return;
            }

            var maxAttempts = app.Configuration.GetValue("Database:MigrateRetryCount", 20);
            var delay = TimeSpan.FromSeconds(app.Configuration.GetValue("Database:MigrateRetryDelaySeconds", 3));

            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    await db.Database.MigrateAsync();
                    Log.Information("Migration tamamlandı ({Attempt}. denemede).", attempt);
                    return;
                }
                // Tam stack trace yerine sadece mesaj: veritabanı ayakta olmadığı için Serilog'un
                // PostgreSQL sink'i de yazamıyor, bu satırlar yalnızca konsola düşüyor.
                catch (Exception ex) when (attempt < maxAttempts)
                {
                    Log.Warning("Veritabanına ulaşılamadı ({Attempt}/{Max}): {Message}. {Delay} sn sonra tekrar denenecek.",
                        attempt, maxAttempts, ex.Message, delay.TotalSeconds);
                    await Task.Delay(delay);
                }
            }
        }
    }
}
