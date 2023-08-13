using MediatR;
using ProjectService.Domain.Event;

namespace ProjectService.Application.Projects.EventHandlers;

public class ProjectSubmittedForApprovalEventHandler : INotificationHandler<ProjectSubmittedForApprovalEvent>
{
    public Task Handle(ProjectSubmittedForApprovalEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}