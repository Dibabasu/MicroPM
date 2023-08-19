using ProjectService.Domain.UnitTest.ProjectTests.Utilites;

namespace ProjectService.Domain.UnitTest.ProjectTests;
public class ProjectTests
{
    private readonly Project _project;
    private readonly Guid _ownerId;
    private readonly Guid _workflowId;
    private readonly Details _details;

    public ProjectTests()
    {
        _ownerId = Guid.NewGuid();
        _workflowId = Guid.NewGuid();
        _details = new Details(ProjectUtilites.ProjectName,ProjectUtilites.ProjectDescription);
        _project = new Project(_details, _ownerId, _workflowId);
    }

    [Fact]
    public void Project_AddComponent_ComponentAdded()
    {
        // Arrange
        var componentDetails = new Details(ProjectUtilites.ComponentName,ProjectUtilites.Componentdescription);

        // Act
        _project.AddComponent(componentDetails);

        // Assert
        Assert.Single(_project.Components);
    }

    [Fact]
    public void Project_RemoveComponent_ComponentRemoved()
    {
        // Arrange
        var componentDetails = new Details(ProjectUtilites.ComponentName,ProjectUtilites.Componentdescription);
        _project.AddComponent(componentDetails);
        var componentId = _project.Components.First().Id;

        // Act
        _project.RemoveComponent(componentId);

        // Assert
        Assert.Empty(_project.Components);
    }

    [Fact]
    public void Project_AddUser_UserAdded()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        _project.AddUser(userId, UserRole.user);

        // Assert
        Assert.Single(_project.ProjectUsers);
    }

    [Fact]
    public void Project_RemoveUser_UserRemoved()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _project.AddUser(userId, UserRole.user);

        // Act
        _project.RemoveUser(userId);

        // Assert
        Assert.Empty(_project.ProjectUsers);
    }

    [Fact]
    public void Project_AddUsers_UsersAdded()
    {
        // Arrange
        var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        // Act
        _project.AddUsers(userIds);

        // Assert
        Assert.Equal(2, _project.ProjectUsers.Count);
    }

    [Fact]
    public void Project_AddAdmin_AdminAdded()
    {
        // Arrange
        var adminId = Guid.NewGuid();

        // Act
        _project.AddAdmin(adminId);

        // Assert
        Assert.Single(_project.ProjectUsers);
        Assert.Equal(UserRole.admin, _project.ProjectUsers.First().UserRole);
    }

    [Fact]
    public void Project_AssignWorkflow_WorkflowAssigned()
    {
        // Arrange
        var newWorkflowId = Guid.NewGuid();

        // Act
        _project.AssignWorkflow(newWorkflowId);

        // Assert
        Assert.Equal(newWorkflowId, _project.WorkflowId);
    }

    [Fact]
    public void Project_RemoveAdmin_AdminRemoved()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        _project.AddAdmin(adminId);

        // Act
        _project.RemoveAdmin(adminId);

        // Assert
        Assert.Empty(_project.ProjectUsers);
    }

    [Fact]
    public void Project_UpdateDetails_DetailsUpdated()
    {
        // Arrange
        var newDetails = new  Details(ProjectUtilites.ProjectName,ProjectUtilites.ProjectDescription);

        // Act
        _project.UpdateDetails(newDetails);

        // Assert
        Assert.Equal(newDetails, _project.ProjectDetails);
    }

    [Fact]
    public void Project_UpdateWorkFlow_WorkFlowUpdated()
    {
        // Arrange
        var newWorkflowId = Guid.NewGuid();

        // Act
        _project.UpdateWorkFlow(newWorkflowId);

        // Assert
        Assert.Equal(newWorkflowId, _project.WorkflowId);
    }

    [Fact]
    public void Project_RemoveUsers_UsersRemoved()
    {
        // Arrange
        var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        _project.AddUsers(userIds);

        // Act
        _project.RemoveUsers(userIds);

        // Assert
        Assert.Empty(_project.ProjectUsers);
    }

    [Fact]
    public void Project_ChangeStatus_StatusChanged()
    {
        // Arrange
        var newStatus = ProjectStatus.approved;

        // Act
        _project.ChangeStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, _project.ProjectStatus);
    }
}
