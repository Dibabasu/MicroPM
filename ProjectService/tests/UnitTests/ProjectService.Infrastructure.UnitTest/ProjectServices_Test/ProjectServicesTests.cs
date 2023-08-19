using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Entity;
using ProjectService.Infrastructure.Persistence;
using ProjectService.Infrastructure.Persistence.Interceptors;
using ProjectService.Infrastructure.Services;

namespace ProjectService.Infrastructure.UnitTest.ProjectServices_Test;

public class ProjectServicesTests
{
    private readonly ProjectServices _projectServices;
    private readonly ProjectServiceDbContext _context;

    public ProjectServicesTests()
    {
        var options = new DbContextOptionsBuilder<ProjectServiceDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectServiceTestDb")
            .Options;

        var mockMediator = Substitute.For<IMediator>();
        var mockDateTime = Substitute.For<IDateTime>();
        mockDateTime.UtcNow.Returns(DateTime.UtcNow);

        var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor(mockDateTime);

        _context = new ProjectServiceDbContext(options, auditableEntitySaveChangesInterceptor, mockMediator);
        _projectServices = new ProjectServices(_context);
    }

    [Fact]
    public async Task ProjectServices_AddProject_ProjectAddedToDatabase()
    {
        // Arrange
        var project = new Project(new Details("Test Project", "This is a test project"), Guid.NewGuid(), Guid.NewGuid());

        // Act
        var projectId = await _projectServices.AddProject(project);

        // Assert
        var addedProject = await _context.Projects.FindAsync(projectId);
        Assert.NotNull(addedProject);
        Assert.Equal("Test Project", addedProject.ProjectDetails.Name);
    }

    [Fact]
    public async Task ProjectServices_DeleteProjectById_ProjectRemovedFromDatabase()
    {
        // Arrange
        var project = new Project(new Details("Test Project", "This is a test project"), Guid.NewGuid(), Guid.NewGuid());
        var projectId = await _projectServices.AddProject(project);

        // Act
        var result = await _projectServices.DeleteProjectById(project, default);

        // Assert
        Assert.True(result);
        var deletedProject = await _context.Projects.FindAsync(projectId);
        Assert.Null(deletedProject);
    }

    [Fact]
    public async Task ProjectServices_GetProjectById_ProjectReturnedIfExists()
    {
        // Arrange
        var project = new Project(new Details("Test Project", "This is a test project"), Guid.NewGuid(), Guid.NewGuid());
        var projectId = await _projectServices.AddProject(project);

        // Act
        var fetchedProject = await _projectServices.GetProjectByIdAsync(projectId, default);

        // Assert
        Assert.NotNull(fetchedProject);
        Assert.Equal(projectId, fetchedProject.Id);
    }

    [Fact]
    public async Task ProjectServices_GetProjectByName_ProjectReturnedIfExists()
    {
        // Arrange
        var project = new Project(new Details("Test Project", "This is a test project"), Guid.NewGuid(), Guid.NewGuid());
        await _projectServices.AddProject(project);

        // Act
        var fetchedProject = await _projectServices.GetProjectByNameAsync("Test Project", default);

        // Assert
        Assert.NotNull(fetchedProject);
        Assert.Equal("Test Project", fetchedProject.ProjectDetails.Name);
    }
    [Fact]
    public async Task ProjectServices_UpdateProject_ProjectUpdatedInDatabase()
    {
        // Arrange
        var project = new Project(new Details("Test Project", "This is a test project"), Guid.NewGuid(), Guid.NewGuid());
        var projectId = await _projectServices.AddProject(project);
        project.UpdateDetails(new Details("Updated Project", "This is an updated test project"));

        // Act
        await _projectServices.UpdateProject(project, default);

        // Assert
        var updatedProject = await _context.Projects.FindAsync(projectId);
        Assert.NotNull(updatedProject);
        Assert.Equal("Updated Project", updatedProject.ProjectDetails.Name);
    }

    [Fact]
    public async Task ProjectServices_GetProjects_ReturnsFilteredProjects()
    {

        // Arrange
        _context.Projects.RemoveRange(_context.Projects);
        var project1 = new Project(new Details("Test Project 1", "This is a test project"), Guid.NewGuid(), Guid.NewGuid());
        var project2 = new Project(new Details("Test Project 2", "This is another test project"), Guid.NewGuid(), Guid.NewGuid());
        await _projectServices.AddProject(project1);
        await _projectServices.AddProject(project2);

        // Act
        var projects = _projectServices.GetProjects(null, null, null, null, "projectName");

        // Assert
        Assert.Equal(2, projects.Count());
    }

    [Fact]
    public async Task ProjectServices_GetProjectById_ReturnsNullIfProjectDoesNotExist()
    {
        // Act
        var project = await _projectServices.GetProjectByIdAsync(Guid.NewGuid(), default);

        // Assert
        Assert.Null(project);
    }

    [Fact]
    public async Task ProjectServices_GetProjectByName_ReturnsNullIfProjectDoesNotExist()
    {
        // Act
        var project = await _projectServices.GetProjectByNameAsync("Nonexistent Project", default);

        // Assert
        Assert.Null(project);
    }

    [Fact]
    public async Task ProjectServices_DeleteProjectById_ReturnsFalseIfProjectDoesNotExist()
    {
        // Arrange
        var project = new Project(new Details("Test Project", "This is a test project"), Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = await _projectServices.DeleteProjectById(project, default);

        // Assert
        Assert.False(result);
    }

    
    [Fact]
    public async Task ProjectServices_UpdateProject_ThrowsExceptionIfProjectDoesNotExist()
    {
        // Arrange
        var project = new Project(new Details("Test Project", "This is a test project"), Guid.NewGuid(), Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _projectServices.UpdateProject(project, default));
    }
}