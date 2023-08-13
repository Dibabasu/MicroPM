using FluentValidation;

namespace ProjectService.Application.Projects.Commands.UpdateUserGroupsToProject;

public class UpdateUserGroupsToProjectCommandValidator : AbstractValidator<UpdateUserGroupsToProjectCommand>
{
    public UpdateUserGroupsToProjectCommandValidator()
    {
        RuleFor(x => x.UserGroupNames).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Nameis required.");
        RuleFor(x => x.UserGroupNames)
           .Must(userGroupNames => userGroupNames != null && userGroupNames.Count <= 5)
           .WithMessage("Maximum 5 user groups at a time can be added to project.");
        RuleFor(x => x.RequestType).NotEmpty().WithMessage("Type is required.");
    }
}