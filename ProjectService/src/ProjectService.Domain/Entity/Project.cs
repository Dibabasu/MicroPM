using ProjectService.Domain.Common;
using ProjectService.Domain.Event;

namespace ProjectService.Domain.Entity;
public class Project : AuditableEntity
{
    public Guid ProjectId { get; private set; }
    public Details ProjectDetails { get; private set; }
    public Guid OwnerId { get; private set; }
    public ICollection<Component> Components { get; private set; } = new HashSet<Component>();
    public Guid WorkflowId { get; private set; }

    public ICollection<ProjectUser> ProjectUsers { get; private set; } = new HashSet<ProjectUser>();
    public ProjectStatus ProjectStatus { get; private set; }

    public Project(Details projectDetails, Guid ownerId, Guid workflowId)
    {
        ProjectId = Guid.NewGuid();
        ProjectDetails = projectDetails;
        OwnerId = ownerId;
        Components = new List<Component>();
        WorkflowId = workflowId;
        ProjectUsers = new List<ProjectUser>();

        AddDomainEvent(new ProjectCreatedEvent(this));
    }
    public void AddComponent(Details componentDetails)
    {
        var component = new Component(componentDetails);
        Components.Add(component);
    }
    public void RemoveComponent(Guid componentId)
    {
        var component = Components.FirstOrDefault(c => c.ComponentId == componentId);
        if (component != null)
        {
            Components.Remove(component);
        }
    }

    public void AddUser(Guid userId, UserRole role)
    {
        if (!ProjectUsers.Any(pu => pu.UserId == userId))
        {
            ProjectUsers.Add(new ProjectUser { UserId = userId, UserRole = role });
        }
    }
    public void AddUsers(List<Guid> userIds)
    {
        var existingUserIds = ProjectUsers.Select(pu => pu.UserId).ToHashSet();

        foreach (var userId in userIds)
        {
            if (!existingUserIds.Contains(userId))
            {
                ProjectUsers.Add(new ProjectUser { UserId = userId, UserRole = UserRole.user });
            }
        }
    }


    public void AddAdmin(Guid adminId)
    {
        AddUser(adminId, UserRole.admin);
    }

    public void AssignWorkflow(Guid workflowId)
    {
        WorkflowId = workflowId;
    }



    public void RemoveUser(Guid userId)
    {
        var user = ProjectUsers.FirstOrDefault(pu => pu.UserId == userId);
        if (user != null)
        {
            ProjectUsers.Remove(user);
        }
    }
    public void RemoveAdmin(Guid adminId)
    {
        RemoveUser(adminId);
    }
    public void UpdateDetails(Details projectdetails)
    {
        ProjectDetails= projectdetails;
    }
    public void UpdateWorkFlow(Guid workflowId)
    {
        WorkflowId = workflowId;
    }
    public void RemoveUsers(List<Guid> userIds)
    {
        var usersToRemove = ProjectUsers.Where(pu => userIds.Contains(pu.UserId)).ToList();

        foreach (var user in usersToRemove)
        {
            ProjectUsers.Remove(user);
        }
    }
    public void ChangeStatus(ProjectStatus newStatus)
    {
        ProjectStatus = newStatus;
    }
    private Project() { }
}