using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProjectService.Application.Common.Behaviours;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Services;
using Serilog;

namespace ProjectService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext() // Include context information
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}> {Properties:j}{NewLine}{Exception}") // Log to the console
        .CreateLogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddTransient<IValidationService, ValidationService>();

        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        return services;


    }
}