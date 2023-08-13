using MediatR;
using OneOf;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.Projects.Commands.UpdateWorkFlowToProject;

public class UpdateWorkFlowToProjectCommand : IRequest<OneOf<Unit, ProjectServiceException>>
{
    public Guid ProjectId { get; set; }
    public string WorkFlowName { get; set; } = string.Empty;
}
public class UpdateWorkFlowToProjectCommandHandler : IRequestHandler<UpdateWorkFlowToProjectCommand, OneOf<Unit, ProjectServiceException>>
{
    private readonly IProjectService _projectService;
    private readonly IWorkflowService _workflowService;

    public UpdateWorkFlowToProjectCommandHandler(IProjectService projectService, IWorkflowService workflowService)
    {
        _projectService = projectService;
        _workflowService = workflowService;
    }

    public async Task<OneOf<Unit, ProjectServiceException>> Handle(UpdateWorkFlowToProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(request.ProjectId, cancellationToken);
            if(project == null)
            {
                throw new NotFoundException("Project", request.ProjectId);
            }
            var workflow = await _workflowService.GetWorkflowByNameAsync(request.WorkFlowName, cancellationToken);
            if (workflow == Guid.Empty)
            {
                throw new NotFoundException("Workflow", request.WorkFlowName);
            }
            project.UpdateWorkFlow(workflow);

            await _projectService.UpdateProject(project, cancellationToken);

            return Unit.Value;
        }
        catch (ProjectServiceException ex)
        {
            return ex;
        }
    }
}