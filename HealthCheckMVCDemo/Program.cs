using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// HealthCheck registration
builder.Services.AddHealthChecks()
    .AddCheck("View", () => HealthCheckResult.Healthy("View check worked well."), new[] { "views" })
    .AddCheck("Controller", () => HealthCheckResult.Healthy("Controller check worked well."), new[] { "controller" })
    .AddCheck("Database", () => HealthCheckResult.Healthy("Database check worked well."), new[] { "db", "sql" })
    .AddCheck("Service", () => HealthCheckResult.Healthy("Service check worked well."), new[] { "service", "api" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
