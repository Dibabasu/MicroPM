using FluentValidation;

namespace ProjectService.Application.Projects.Commands.UpdateUsersToProject;

public class UpdateUsersToProjectCommandValidator : AbstractValidator<UpdateUsersToProjectCommand>
{
    public UpdateUsersToProjectCommandValidator()
    {
        RuleFor(x => x.UserNames).NotEmpty().WithMessage("UserNames are required.");
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project Id is required.");
        RuleFor(x => x.UserNames)
            .Must(UserNames => UserNames != null && UserNames.Count <= 5)
            .WithMessage("Maximum 5 user groups at a time can be added to project.");
        RuleFor(x => x.RequestType).NotEmpty().WithMessage("RequestType is required.");

    }
}