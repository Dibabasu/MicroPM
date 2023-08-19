using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Errors;
using OneOf;
using ProjectService.Application.UnitTest.CreateProject_Test.Utility;
using ProjectService.Domain.Entity;
using ProjectService.Application.DTOs;

namespace ProjectService.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandHandlerTests
{
    private readonly CreateProjectCommandHandler _handler;
    private readonly IValidationService _validationService;
    private readonly IProjectService _projectService;

    public CreateProjectCommandHandlerTests()
    {
        _validationService = Substitute.For<IValidationService>();
        _projectService = Substitute.For<IProjectService>();
        _handler = new CreateProjectCommandHandler(_validationService, _projectService);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsProjectId()
    {
        var command = new CreateProjectCommand { ProjectName = "Test", ProjectDescription = "Test", Owner = "Test" };
        var result = await _handler.Handle(command, new CancellationToken());
        Assert.IsType<OneOf<Guid, ProjectServiceException>>(result);
        Assert.True(result.IsT0);
    }

    [Fact]
    public async Task Handle_InvalidOwner_ThrowsProjectServiceException()
    {
        var command = CreateCommand(
            name: CreateProjectUtility.ProjectName,
            description: CreateProjectUtility.ProjectDescription,
            owner: string.Empty);

        _validationService.When(x => x.ValidateUser(command.Owner)).Throw(new NotFoundException("User"));
        var result = await _handler.Handle(command, new CancellationToken());
        Assert.IsType<OneOf<Guid, ProjectServiceException>>(result);
        Assert.True(result.IsT1);
    }
    [Fact]
    public async Task Handle_InvalidWorkflow_ThrowsProjectServiceException()
    {
        var command = CreateCommand(
            name: "Test",
            description: "Test",
            owner: "Test",
            workflow: "InvalidWorkflow");

        _validationService.When(x => x.ValidateWorkflow(command.Workflow)).Throw(new NotFoundException("Workflow"));
        var result = await _handler.Handle(command, new CancellationToken());
        Assert.IsType<OneOf<Guid, ProjectServiceException>>(result);
        Assert.True(result.IsT1);
    }

    [Fact]
    public async Task Handle_AddAdmin_Success()
    {
        var command = CreateCommand(
            name: "Test",
            description: "Test",
            owner: "Test",
            admin: new List<string> { "Admin1" });

        var result = await _handler.Handle(command, new CancellationToken());
        Assert.IsType<OneOf<Guid, ProjectServiceException>>(result);
        Assert.True(result.IsT0);
    }

    [Fact]
    public async Task Handle_AddUser_Success()
    {
        var command = CreateCommand(
            name: "Test",
            description: "Test",
            owner: "Test",
            users: new List<string> { "User1" });

        var result = await _handler.Handle(command, new CancellationToken());
        Assert.IsType<OneOf<Guid, ProjectServiceException>>(result);
        Assert.True(result.IsT0);
    }

    [Fact]
    public async Task Handle_AddUserGroup_Success()
    {
        var command = CreateCommand(
            name: "Test",
            description: "Test",
            owner: "Test",
            userGroup: new List<string> { "UserGroup1" });

        var result = await _handler.Handle(command, new CancellationToken());
        Assert.IsType<OneOf<Guid, ProjectServiceException>>(result);
        Assert.True(result.IsT0);
    }

    [Fact]
    public async Task Handle_AddComponent_Success()
    {
        var command = CreateCommand(
            name: "Test",
            description: "Test",
            owner: "Test",
            components: new List<CreateComponentDTO> { new CreateComponentDTO("Component1", "Description") });

        var result = await _handler.Handle(command, new CancellationToken());
        Assert.IsType<OneOf<Guid, ProjectServiceException>>(result);
        Assert.True(result.IsT0);
    }

    private static CreateProjectCommand CreateCommand(string name, string description, string owner, string workflow = "", List<string> admin = null, List<string> users = null, List<string> userGroup = null, List<CreateComponentDTO> components = null)
    {
        return new CreateProjectCommand
        {
            ProjectName = name,
            ProjectDescription = description,
            Owner = owner,
            Workflow = workflow,
            Admin = admin,
            Users = users,
            UserGroup = userGroup,
            Components = components
        };
    }

}
