using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// HealthCheck registration
builder.Services.AddHealthChecks()
    //.AddCheck("View", () => HealthCheckResult.Healthy("View check worked well."), new[] { "views" })
    //.AddCheck("Controller", () => HealthCheckResult.Healthy("Controller check worked well."), new[] { "controller" })
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("SQLServer"),
        healthQuery: "SELECT 1",
        name: "SQL Server Check",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "sql", "server", "db", "database" })
    .AddNpgSql(
        npgsqlConnectionString: builder.Configuration.GetConnectionString("PGServer"),
        healthQuery: "SELECT 1",
        name: "PostGreSQL Check",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "pgsql", "sql", "db", "postgresql", "npgsql" })
    .AddMongoDb(
        mongodbConnectionString: builder.Configuration.GetConnectionString("MongoServer"),
        name: "MongoDB Check",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "mongodb", "sql", "db", "mongo" });
    // .AddMongoDb(
    //    mongoClientSettings: new MongoDB.Driver.MongoClientSettings { } ,
    //    name: "PostGress SQL Check",
    //    failureStatus: HealthStatus.Degraded,
    //    tags: new[] { "pgsql", "sql", "db", "postgress" });

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
