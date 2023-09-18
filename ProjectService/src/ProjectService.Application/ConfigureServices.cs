using System.Reflection;
using Amazon.SimpleNotificationService;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectService.Application.Common.Behaviours;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Mappings;
using ProjectService.Application.Common.MessagePublisher;
using ProjectService.Application.Common.MessagePublisher.Interfaces;
using ProjectService.Application.Common.Services;
using Serilog;

namespace ProjectService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
    {

        Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext() // Include context information
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}> {Properties:j}{NewLine}{Exception}") // Log to the console
        .CreateLogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

        services.AddSingleton<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
        
        var platforms = configuration.GetSection("MessagePublishers").Get<List<string>>();
        foreach (var platform in platforms)
        {
            switch (platform)
            {
                case "SNS":
                    services.AddSingleton<IMessagePublisher, SnsMessagePublisher>();
                    break;
                case "Azure":
                    services.AddSingleton<IMessagePublisher, AzureMessagePublisher>();
                    break;
                case "Local":
                    services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
                    break;
                default:
                    throw new Exception($"Unsupported message publisher: {platform}");
            }
        }

        MappingConfig.RegisterMappings();
        services.AddMapper();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddTransient<IValidationService, ValidationService>();

        services.AddSingleton<ETagService>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        return services;
    }
}