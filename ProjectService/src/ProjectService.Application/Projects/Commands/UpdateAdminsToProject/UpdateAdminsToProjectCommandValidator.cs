using FluentValidation;

namespace ProjectService.Application.Projects.Commands.UpdateAdminsToProject;

public class UpdateAdminsToProjectCommandValidator : AbstractValidator<UpdateAdminsToProjectCommand>
{
    public UpdateAdminsToProjectCommandValidator()
    {
        RuleFor(x => x.Admins).NotEmpty().WithMessage("Admins are required.");
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project Id required.");
        RuleFor(x => x.Admins)
            .Must(Admins => Admins != null && Admins.Count <= 2)
            .WithMessage("Maximum 2 admins  at a time can be added to project.");
        RuleFor(x => x.RequestType).NotEmpty().WithMessage("RequestType is required.");

    }
}