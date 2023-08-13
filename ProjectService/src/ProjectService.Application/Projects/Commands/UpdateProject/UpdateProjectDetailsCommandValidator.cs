using FluentValidation;

namespace ProjectService.Application.Projects.Commands.UpdateProject;

public class UpdateProjectDetailsCommandValidator : AbstractValidator<UpdateProjectDetailsCommand>
{
    public UpdateProjectDetailsCommandValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Nameis required.");
        RuleFor(x=> x.ProjectId).NotEmpty().WithMessage("Project Id is required.");
    }
}