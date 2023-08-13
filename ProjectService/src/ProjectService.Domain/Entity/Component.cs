using ProjectService.Domain.Common;

namespace ProjectService.Domain.Entity;
public class Component : AuditableEntity
{
    
    public Guid Id { get; private set; }
    public Details Details { get; private set; }
    public Guid ProjectId { get; private set; }
    public Project? Project { get; private set; }

    public Component(Details details)
    {
        Id = Guid.NewGuid();
        Details = details;
    }

    public void UpdateDetails(Details details, Guid projectid, Guid id)
    {
        Id = id;
        Details = details;
        ProjectId = projectid;
    }
    private Component()
    {

    }
}