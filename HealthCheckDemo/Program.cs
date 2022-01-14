using HealthCheckDemo.HealthChecks;
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
builder.Services.AddHealthChecks()
    .AddCheck<ResponseTimeHealthCheck>("Network speed test", tags: new[] { "service" })
    .AddCheck("Database", () => HealthCheckResult.Healthy("Database connection is healthy"), new[] { "database", "sql" });

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
