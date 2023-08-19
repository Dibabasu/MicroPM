using ProjectService.Application.Projects.Commands.CreateProject;

namespace ProjectService.Application.UnitTest.CreateProject_Test.Utility;
public static class CreateProjectUtility
{
    public const string ProjectName = "Project1";
    public const string ProjectDescription = "Project 1 Description";
    public const string Owner = "Onwer 1";
    public static CreateProjectCommand CreateCommand(string name, string description, string owner)
    {
        return new CreateProjectCommand
        {
            ProjectName=name,
            ProjectDescription=description,
            Owner=owner
        };
    }
}