using MediatR;
using ProjectService.Application.Common.MessagePublisher.Interfaces;
using ProjectService.Domain.Event;
using Serilog;

namespace ProjectService.Application.Projects.Eventhandlers;

public class ProjectCreatedEventHandler : INotificationHandler<ProjectCreatedEvent>
{
    private readonly ILogger _logger;
    private readonly IEnumerable<IMessagePublisher> _messagePublishers;

    public ProjectCreatedEventHandler(IEnumerable<IMessagePublisher> messagePublishers)
    {
        _logger = Log.ForContext<ProjectCreatedEventHandler>();
        _messagePublishers = messagePublishers;
    }

    public async Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
    {
        foreach (var publisher in _messagePublishers)
        {
            await publisher.Publish(notification);
        }
    }
}