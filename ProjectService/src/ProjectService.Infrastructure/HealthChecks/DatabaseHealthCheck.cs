using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProjectService.Infrastructure.Persistence;

namespace ProjectService.Infrastructure.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly ProjectServiceDbContext _context;

    public DatabaseHealthCheck(ProjectServiceDbContext context)
    {
        _context = context;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Try to connect to the database
            var connections = await _context.Database.CanConnectAsync(cancellationToken);
            if (connections){
                return HealthCheckResult.Healthy();
            }
            return HealthCheckResult.Unhealthy("Couldn't Connect to database.");
        }
        catch (Exception ex)
        {
            // If we can't connect to the database, return a failed health check.
            return HealthCheckResult.Unhealthy("Database is unreachable", ex);
        }
    }
}