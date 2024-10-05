using ToDosProject.MigrationService;
using ToDosProject.Infraestructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ToDosProject.Domain;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ApiDbInitializer>();

builder.AddServiceDefaults();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(ApiDbInitializer.ActivitySourceName));

builder.AddSqlServerDbContext<AppDbContext>(AppConfiguration.DATABASE);

var app = builder.Build();

app.Run();