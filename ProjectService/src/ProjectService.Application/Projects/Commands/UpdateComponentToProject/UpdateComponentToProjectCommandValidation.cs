using FluentValidation;

namespace ProjectService.Application.Projects.Commands.UpdateComponentToProject;

public class UpdateComponentToProjectCommandValidation: AbstractValidator<UpdateComponentToProjectCommand>
{
public UpdateComponentToProjectCommandValidation()
{
    RuleFor(x => x.ComponentDescription).NotEmpty().WithMessage("Description is required.");
    RuleFor(x => x.ComponentName).NotEmpty().WithMessage("Nameis required.");
    RuleFor(x=> x.ProjectId).NotEmpty().WithMessage("Project Id is required.");
    RuleFor(x=> x.RequestType).NotEmpty().WithMessage("Request Type is required.");
}
}