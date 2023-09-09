using MediatR;
using ProjectService.Application.Common.MessagePublisher.Interfaces;
using ProjectService.Domain.Event;

namespace ProjectService.Application.Projects.EventHandlers;

public class ProjectSubmittedForApprovalEventHandler : INotificationHandler<ProjectSubmittedForApprovalEvent>
{
    private readonly IEnumerable<IMessagePublisher> _messagePublishers;

    public ProjectSubmittedForApprovalEventHandler(IEnumerable<IMessagePublisher> messagePublishers)
    {
        _messagePublishers = messagePublishers;
    }

    public async Task Handle(ProjectSubmittedForApprovalEvent notification, CancellationToken cancellationToken)
    {
        foreach (var publisher in _messagePublishers)
        {
            await publisher.Publish(notification);
        }
    }
}