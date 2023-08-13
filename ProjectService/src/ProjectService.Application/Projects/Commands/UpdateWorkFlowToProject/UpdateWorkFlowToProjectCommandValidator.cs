using FluentValidation;

namespace ProjectService.Application.Projects.Commands.UpdateWorkFlowToProject;

public class UpdateWorkFlowToProjectCommandValidator : AbstractValidator<UpdateWorkFlowToProjectCommand>
{
    public UpdateWorkFlowToProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("ProjectId is required.");
        RuleFor(x => x.WorkFlowName).NotEmpty().WithMessage("Workflow name is required.");
    }
}