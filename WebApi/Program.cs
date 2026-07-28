using Core;
using FluentValidation;
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

    // CORS: allow Angular dev server
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularDevClient", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

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

    // Serilog ile HTTP request loglama
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    await app.seedUserData();

    // Enable CORS policy
    app.UseCors("AllowAngularDevClient");

    app.AddMiddleware();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Uygulama başlatılamadı!");
}
finally
{
    Log.CloseAndFlush();
}
