using API.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Singleton service registration
builder.Services.AddSingleton<ResponseTimeHealthCheck>();

// Healthcheck Services
// https://hamedfathi.me/a-professional-asp.net-core-api-health-check/
// https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

builder.Services.AddHealthChecks()
    .AddCheck<ResponseTimeHealthCheck>("Network speed test", tags: new[] { "service" })
    .AddProcessHealthCheck(
        processName: "WorkerService", 
        predicate: p => p.Length > 0, 
        name: "WorkerService Check",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "process", "service", "worker-service", "background-service" })
    .AddProcessAllocatedMemoryHealthCheck(
        maximumMegabytesAllocated: 512, // max memory to allocate in MB exceeding which results in Unhealthy
        name: "Process memory allocation",
        tags: new[] { "memory", "process-memory", "process" }) 
    .AddDiskStorageHealthCheck(
        setup: s => s.AddDrive("C:\\", 1024), // 1024 MB (1 GB) free minimum
        name: "Disk Storage",
        tags: new[] { "storage", "disk-storage" })
    .AddUrlGroup(
        uri:new Uri("https://dev-portal.caireinc.com/"),
        name: "myCaire portal- Dev",
        tags: new[] { "sites", "uri", "webpages", "dev" })
    .AddUrlGroup(
        uri: new Uri("https://stage-portal.caireinc.com/"),
        name: "myCaire Portal- Staging",
        tags: new[] { "sites", "uri", "webpages", "stage", "staging" })
    .AddUrlGroup(
        uri: new Uri("https://portal.mycaire.com/"),
        name: "myCaire Portal- Production",
        tags: new[] { "sites", "uri", "webpages", "prod", "main", "production" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/quickhealth", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/services", new HealthCheckOptions
{
    Predicate = reg => reg.Tags.Contains("service"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
