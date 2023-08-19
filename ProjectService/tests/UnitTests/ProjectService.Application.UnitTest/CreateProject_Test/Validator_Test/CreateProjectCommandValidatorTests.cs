using FluentValidation.TestHelper;
using ProjectService.Application.Projects.Commands.CreateProject;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Entity;
using ProjectService.Application.UnitTest.CreateProject_Test.Utility;

namespace ProjectService.Application.UnitTest.CreateProject_Test.Validator_Test;
public class CreateProjectCommandValidatorTests
{
    private readonly CreateProjectCommandValidator _validator;
    private readonly IProjectService _projectService;

    public CreateProjectCommandValidatorTests()
    {
        _projectService = Substitute.For<IProjectService>();
        _validator = new CreateProjectCommandValidator(_projectService);
    }

    [Fact]
    public async Task Validate_ValidRequest_ReturnsSuccess()
    {
        var command = CreateProjectUtility.CreateCommand
         (
            name: CreateProjectUtility.ProjectName,
            description: CreateProjectUtility.ProjectDescription,
            owner: CreateProjectUtility.Owner
        );
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyDescription_ReturnsFailure()
    {
        var command = CreateProjectUtility.CreateCommand
        (
            name: CreateProjectUtility.ProjectName,
            description: "",
            owner: CreateProjectUtility.Owner
        );
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.ProjectDescription);
    }

    [Fact]
    public async Task Validate_EmptyOwner_ReturnsFailure()
    {
        var command = CreateProjectUtility.CreateCommand
        (
            name: CreateProjectUtility.ProjectName,
            description: CreateProjectUtility.ProjectDescription,
            owner: ""
        );
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Owner);
    }

    [Fact]
    public async Task Validate_EmptyName_ReturnsFailure()
    {
        var command = CreateProjectUtility.CreateCommand
        (
            name: "",
            description: CreateProjectUtility.ProjectDescription,
            owner: CreateProjectUtility.Owner
        );
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.ProjectName);
    }

    [Fact]
    public async Task Validate_ProjectNameAlreadyExists_ReturnsFailure()
    {
        var command = CreateProjectUtility.CreateCommand
        (
            name: CreateProjectUtility.ProjectName,
            description: CreateProjectUtility.ProjectDescription,
            owner: CreateProjectUtility.Owner
        );

        var projectDetails = new Details(CreateProjectUtility.ProjectName, CreateProjectUtility.ProjectDescription);
        _projectService.GetProjectByNameAsync(command.ProjectName,
            Arg.Any<CancellationToken>()).Returns(new Project(projectDetails, Guid.NewGuid(), Guid.NewGuid()));
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.ProjectName);
    }
    [Fact]
    public async Task Validate_NullName_ReturnsFailure()
    {

        var command = CreateProjectUtility.CreateCommand
        (
            name: null,
            description: CreateProjectUtility.ProjectDescription,
            owner: CreateProjectUtility.Owner
        );
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.ProjectName);
    }

    [Fact]
    public async Task Validate_NullDescription_ReturnsFailure()
    {
        var command = CreateProjectUtility.CreateCommand
        (
            name: CreateProjectUtility.ProjectName,
            description: null,
            owner: CreateProjectUtility.Owner
        );
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.ProjectDescription);
    }

    [Fact]
    public async Task Validate_NullOwner_ReturnsFailure()
    {
        var command = CreateProjectUtility.CreateCommand
        (
            name: CreateProjectUtility.ProjectName,
            description: CreateProjectUtility.ProjectDescription,
            owner: null
        );
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Owner);
    }
   
}
