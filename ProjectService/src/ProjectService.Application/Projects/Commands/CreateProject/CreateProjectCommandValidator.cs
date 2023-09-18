using FluentValidation;
using ProjectService.Application.Common.Interfaces;

namespace ProjectService.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    private readonly IProjectService _projectService;
    public CreateProjectCommandValidator(IProjectService projectService)
    {
        _projectService = projectService;
        RuleFor(x => x.ProjectDescription).NotEmpty().WithMessage("Project Description is required.");
        RuleFor(x => x.Owner).NotEmpty().WithMessage("Owner is required.");
        RuleFor(x => x.ProjectName).NotEmpty().WithMessage("Project name is required.")
             .MustAsync(async (name, cancellation) => !await DoesProjectAlreadyExist(name, cancellation))
             .WithMessage("Project name already exists.");

        RuleFor(x => x.Owner).NotEmpty().WithMessage("Owner field cannot be empty.");
        RuleFor(x => x.Owner)
            .Must(owner => string.IsNullOrWhiteSpace(owner) || !owner.Contains(' '))
            .WithMessage("Owner field cannot contain white spaces if it has a value.");
    }
    private async Task<bool> DoesProjectAlreadyExist(string projectName, CancellationToken ct)
    {
        return await _projectService.GetProjectByNameAsync(projectName, ct)! is not null;
    }
}