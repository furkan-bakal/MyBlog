using Core;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using System.Threading.RateLimiting;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using WebApi.Extensions;
using WebApi.Filters;

// Serilog Bootstrap Logger - uygulama başlamadan önce hataları yakalamak için
Serilog.Debugging.SelfLog.Enable(Console.Error);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Uygulama başlatılıyor...");

    var builder = WebApplication.CreateBuilder(args);

    // Serilog konfigürasyonu
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        var connectionString = context.Configuration.GetConnectionString("PostgreSqlConnection");

        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} | {Message:lj}{NewLine}{Exception}")
            .WriteTo.PostgreSQL(
                connectionString: connectionString!,
                restrictedToMinimumLevel: LogEventLevel.Information,
                tableName: "AppLogs",
                needAutoCreateTable: true,
                columnOptions: new Dictionary<string, ColumnWriterBase>
                {
                    { "message", new RenderedMessageColumnWriter() },
                    { "message_template", new MessageTemplateColumnWriter() },
                    { "level", new LevelColumnWriter() },
                    { "timestamp", new TimestampColumnWriter() },
                    { "exception", new ExceptionColumnWriter() },
                    { "log_event", new LogEventSerializedColumnWriter() },
                    { "source_context", new SinglePropertyColumnWriter("SourceContext") }
                });
    });

    // Add services to the container.
    builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>());
    builder.Services.AddScoped<NotFoundFilter>();

    // Reverse proxy arkasında istemcinin gerçek IP'si ve şeması X-Forwarded-* başlıklarından
    // okunur. Hız sınırı ve ziyaretçi hash'i RemoteIpAddress'e dayandığı için bu şart:
    // aksi halde tüm trafik proxy'nin tek IP'si gibi görünür ve aynı kovayı paylaşır.
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        // Proxy compose ağında; IP'si sabit olmadığı için bilinen-proxy listesi temizleniyor.
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

    // CORS: tek origin (reverse proxy) kurulumunda gerek yok, bu yüzden origin listesi
    // konfigürasyondan gelir ve boşsa politika hiç kaydedilmez — AllowCredentials()
    // boş origin listesiyle çalışma zamanında patlar.
    var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()?
        .Where(origin => !string.IsNullOrWhiteSpace(origin))
        .ToArray() ?? [];

    if (corsOrigins.Length > 0)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Default", policy =>
            {
                policy.WithOrigins(corsOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });
    }

    // Identity'nin güvenlik damgası/token'ları Data Protection'a dayanır. Anahtarlar
    // konteyner içinde kalırsa her yeniden başlatmada oturumlar düşer; kalıcı bir
    // dizine yazılır (yol verilmezse varsayılan davranış korunur).
    var dataProtectionKeysPath = builder.Configuration["DataProtection:KeysPath"];
    if (!string.IsNullOrWhiteSpace(dataProtectionKeysPath))
    {
        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath))
            .SetApplicationName("MyBlog");
    }

    builder.Services.AddHealthChecks();

    // Beğeni ve yorum anonim olduğu için tek koruma IP başına hız sınırı.
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        options.AddPolicy(RateLimitPolicies.ArticleLike, httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1)
                }));

        options.AddPolicy(RateLimitPolicies.ArticleComment, httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 3,
                    Window = TimeSpan.FromMinutes(1)
                }));
    });

    builder.Services.AddRepository(builder.Configuration);
    builder.Services.AddService(builder.Configuration);

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();

    // İlk middleware olmalı: request logging, hız sınırı ve ziyaretçi hash'i
    // bundan sonra gerçek istemci IP'sini görebilsin.
    app.UseForwardedHeaders();

    // Serilog ile HTTP request loglama
    app.UseSerilogRequestLogging();

    // Şema seed'den önce hazır olmalı.
    await app.MigrateDatabaseAsync();

    // Configure the HTTP request pipeline.
    await app.seedUserData();

    // Enable CORS policy
    if (corsOrigins.Length > 0)
    {
        app.UseCors("Default");
    }

    app.AddMiddleware();

    app.Run();
}
// HostAbortedException'ı dışarıda bırak: `dotnet ef migrations add` host'u kurup
// bu istisnayla iptal eder; aksi halde her design-time EF komutu 1 ile çıkardı.
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Uygulama başlatılamadı!");
    // Bu satır olmadan başlangıç hatası exit code 0 döner; Docker uygulamayı
    // "başarıyla bitti" sayar ve restart politikası hiç devreye girmez.
    Environment.ExitCode = 1;
}
finally
{
    Log.CloseAndFlush();
}
