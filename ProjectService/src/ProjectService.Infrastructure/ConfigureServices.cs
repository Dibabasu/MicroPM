using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectService.Application.Common;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Infrastructure.Persistence;
using ProjectService.Infrastructure.Persistence.Interceptors;
using ProjectService.Infrastructure.Services;

namespace ProjectService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var ProjectServiceConfig = configuration.GetSection("projectDb").Get<ProjectServiceSettings>() ?? throw new EmptyOrNullException("ProjectServiceConfig");

        services.AddDbContext<ProjectServiceDbContext>(options =>
            options.UseSqlServer(ProjectServiceConfig.ConnectionString));

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