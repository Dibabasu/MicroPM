using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;

namespace ProjectService.Domain.Event;

public class ProjectCreatedEvent : BaseEvent
{
    public Project Project { get; }

    public ProjectCreatedEvent(Project project)
    {
        Project = project;
    }
}