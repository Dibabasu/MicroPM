using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;

namespace ProjectService.Domain.Event;

public class ProjectSubmittedForApprovalEvent : BaseEvent
{
    public ProjectSubmittedForApprovalEvent(Project project)
    {
        Project = project;
    }
    public Project Project { get; }
}