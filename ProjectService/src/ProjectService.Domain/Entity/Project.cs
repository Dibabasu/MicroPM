using ProjectService.Domain.Common;
using ProjectService.Domain.Event;

namespace ProjectService.Domain.Entity;
public class Project : AuditableEntity
{
    public Details ProjectDetails { get; private set; }
    public string OwnerId { get; private set; }
    public ICollection<Component> Components { get; private set; } = new HashSet<Component>();
    public Guid WorkflowId { get; private set; }

    public ICollection<ProjectUser> ProjectUsers { get; private set; } = new HashSet<ProjectUser>();
    public ProjectStatus ProjectStatus { get; private set; }

    public Project(Details projectDetails, string ownerId, Guid workflowId)
    {
        Id = Guid.NewGuid();
        ProjectDetails = projectDetails;
        OwnerId = ownerId;
        Components = new List<Component>();
        WorkflowId = workflowId;
        ProjectUsers = new List<ProjectUser>();

        AddDomainEvent(new ProjectSubmittedForApprovalEvent(this));
    }
    public void AddComponent(Details componentDetails)
    {
        try
        {
            var component = new Component(componentDetails);
            Components.Add(component);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public void RemoveComponent(Guid componentId)
    {
        var component = Components.FirstOrDefault(c => c.Id == componentId);
        if (component != null)
        {
            Components.Remove(component);
        }
    }

    public void AddUser(string username, UserRole role)
    {
        var existingUser = ProjectUsers.FirstOrDefault(pu => pu.UserName == username
        && pu.ProjectId == this.Id);
        if (existingUser == null)
        {
            ProjectUsers.Add(new ProjectUser { UserName = username, UserRole = role, ProjectId = this.Id });
        }
        else
        {
            throw new InvalidOperationException($"User - {username} already exists in this project.");
        }
    }

    public void AddUsers(List<string> usernames)
    {
        foreach (var username in usernames)
        {
            AddUser(username, UserRole.user);
        }
    }

    public void AddAdmin(string adminUsername)
    {
        var existingAdmin = ProjectUsers.FirstOrDefault(pu => pu.UserName == adminUsername
            && pu.ProjectId == this.Id
            && pu.UserRole == UserRole.admin);
        if (existingAdmin == null)
        {
            AddUser(adminUsername, UserRole.admin);
        }
        else
        {
            throw new InvalidOperationException($"Admin - {adminUsername} already exists in the project.");
        }
    }

    public void RemoveAdmin(string adminUsername)
    {
        var admin = ProjectUsers.FirstOrDefault(pu => pu.UserName == adminUsername
        && pu.UserRole == UserRole.admin
         && pu.ProjectId == this.Id);
        if (admin != null)
        {
            ProjectUsers.Remove(admin);
        }
        else
        {
            throw new InvalidOperationException($"Admin - {adminUsername} does not exist in the project.");
        }
    }
    public void RemoveUser(string username)
    {
        var existingUser = ProjectUsers.FirstOrDefault(pu => pu.UserName == username
         && pu.ProjectId == this.Id);
        if (existingUser != null)
        {
            ProjectUsers.Remove(existingUser);
        }
    }


    public void AssignWorkflow(Guid workflowId)
    {
        WorkflowId = workflowId;
    }





    public void UpdateDetails(Details projectdetails)
    {
        ProjectDetails = projectdetails;
    }
    public void UpdateWorkFlow(Guid workflowId)
    {
        WorkflowId = workflowId;
    }
    public void RemoveUsers(List<string> userIds)
    {
        foreach (var user in userIds)
        {
            RemoveUser(user);
        }
    }
    public void ChangeStatus(ProjectStatus newStatus)
    {
        ProjectStatus = newStatus;
        if (newStatus == ProjectStatus.approved)
        {
            AddDomainEvent(new ProjectCreatedEvent(this));
        }
    }
    private Project() { }
}