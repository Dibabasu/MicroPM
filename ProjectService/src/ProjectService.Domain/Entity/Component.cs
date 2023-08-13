using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entity;
public class Component : AuditableEntity
{
    
    public Guid ComponentId { get; private set; }
    public Details ComponentDetails { get; private set; }
    public Guid ProjectId { get; private set; }
    public Project? Project { get; private set; }

    public Component(Details details)
    {
        ComponentId = Guid.NewGuid();
        ComponentDetails = details;
    }

    public void UpdateDetails(Details details, Guid projectid, Guid id)
    {
        ComponentId = id;
        ComponentDetails = details;
        ProjectId = projectid;
    }
    private Component()
    {

    }
}