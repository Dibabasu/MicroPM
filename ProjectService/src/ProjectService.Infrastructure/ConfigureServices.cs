using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Infrastructure.HealthChecks;
using ProjectService.Infrastructure.Persistence;
using ProjectService.Infrastructure.Persistence.Interceptors;
using ProjectService.Infrastructure.Services;
using ProjectService.Application.Common.Settings;
using ProjectService.Infrastructure.HttpClients;
using ProjectService.Infrastructure.Common;

namespace ProjectService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<UserServiceSettings>(configuration.GetSection("UserService"));
        var ProjectServiceConfig = configuration.GetSection("projectDb").Get<ProjectServiceSettings>() ?? throw new EmptyOrNullException("ProjectServiceConfig");

        services.AddDbContext<ProjectServiceDbContext>(options =>
            options.UseSqlServer(ProjectServiceConfig.ConnectionString));


        services.AddHealthChecks()
         .AddSqlServer(
         connectionString: ProjectServiceConfig.ConnectionString,
         healthQuery: "SELECT 1;",
         name: "sql",
         failureStatus: HealthStatus.Degraded,
         tags: new string[] { "db", "sql", "sqlserver" });

        services.AddHealthChecks()
       .AddCheck<DatabaseHealthCheck>("Database");

        services.AddHttpClient<UserServiceClient>(client =>
        {
            var userServiceSettings = configuration.GetSection("UserService").Get<UserServiceSettings>();
            client.BaseAddress = new Uri(userServiceSettings!.BaseUrl);
        })
        .AddPolicyHandler(PollyPolicyFactory.GetRetryPolicy());

        services.AddHttpClient<UserGroupServiceClient>(client =>
        {
            var userServiceSettings = configuration.GetSection("UserService").Get<UserServiceSettings>();
            client.BaseAddress = new Uri(userServiceSettings!.BaseUrl);
        })
        .AddPolicyHandler(PollyPolicyFactory.GetRetryPolicy());



        services.AddTransient<IComponentService, ComponentService>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IProjectService, ProjectServices>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserGroupService, UserGroupService>();
        services.AddTransient<IWorkflowService, WorkflowService>();

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        return services;
    }
}