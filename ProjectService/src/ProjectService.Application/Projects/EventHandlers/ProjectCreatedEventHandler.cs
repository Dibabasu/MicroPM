using MediatR;
using ProjectService.Domain.Event;
using Serilog;

namespace ProjectService.Application.Projects.Eventhandlers;

public class ProjectCreatedEventHandler : INotificationHandler<ProjectCreatedEvent>
{
    private readonly ILogger _logger;

    public ProjectCreatedEventHandler()
    {
        _logger = Log.ForContext<ProjectCreatedEventHandler>();
    }

    public async Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Project Created Event Handler{0}",notification);
    
    }
}