using MediatR;
using Microsoft.Extensions.Configuration;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.MessagePublisher.Interfaces;
using ProjectService.Domain.Event;
using Serilog;

namespace ProjectService.Application.Projects.Eventhandlers;

public class ProjectCreatedEventHandler : INotificationHandler<ProjectCreatedEvent>
{
    private readonly ILogger _logger;
    private readonly IEnumerable<IMessagePublisher> _messagePublishers;
    private readonly string _topicname;
    public ProjectCreatedEventHandler(IEnumerable<IMessagePublisher> messagePublishers, IConfiguration configuration)
    {
        _logger = Log.ForContext<ProjectCreatedEventHandler>();
        _messagePublishers = messagePublishers;
        _topicname = configuration.GetSection("Topics:ProjectCreated").Value ?? string.Empty;
    }

    public async Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
    {
        if (!String.IsNullOrEmpty(_topicname))
        {
            foreach (var publisher in _messagePublishers)
            {
                await publisher.Publish(notification, _topicname);
            }
        }
        throw new EmptyOrNullException("Topic not setup");
    }
}