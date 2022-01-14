namespace HealthCheckDemo.HealthChecks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

public class ResponseTimeHealthCheck : IHealthCheck
{
    private Random rnd = new Random();
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var responseTime = rnd.Next(1, 300);
        return Task.FromResult(
                responseTime < 100 ?
                HealthCheckResult.Healthy($"Response time is good ({responseTime}ms).") :
                responseTime < 200 ?
                HealthCheckResult.Degraded($"Response time is poor ({responseTime}ms).") :
                HealthCheckResult.Unhealthy($"Response time is bad ({responseTime}ms).")
            );
    }
}