using MediatR;
using Microsoft.Extensions.Configuration;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.MessagePublisher.Interfaces;
using ProjectService.Domain.Event;

namespace ProjectService.Application.Projects.EventHandlers;

public class ProjectSubmittedForApprovalEventHandler : INotificationHandler<ProjectSubmittedForApprovalEvent>
{
    private readonly IEnumerable<IMessagePublisher> _messagePublishers;
    private readonly string _topicname;

    public ProjectSubmittedForApprovalEventHandler(IEnumerable<IMessagePublisher> messagePublishers, IConfiguration configuration)
    {
        _messagePublishers = messagePublishers;
        _topicname = configuration.GetSection("Topics:ProjectSubmittedForApproval").Value ?? string.Empty;
    }
    public async Task Handle(ProjectSubmittedForApprovalEvent notification, CancellationToken cancellationToken)
    {
        if (String.IsNullOrEmpty(_topicname))
        {
            throw new EmptyOrNullException("Topic not setup");
        }
        foreach (var publisher in _messagePublishers)
        {
            await publisher.Publish(notification, _topicname);
        }
    }
}