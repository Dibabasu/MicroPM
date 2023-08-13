using MediatR;
using OneOf;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.Projects.Commands.UpdateComponentToProject;

public class UpdateComponentToProjectCommand : IRequest<OneOf<Unit, ProjectServiceException>>
{
    public Guid ProjectId { get; set; }
    public string ComponentName { get; set; } = string.Empty;
    public string ComponentDescription { get; set; } = string.Empty;
    public RequestType RequestType { get; set; }
}

public class UpdateComponentToProjectCommandHandler : IRequestHandler<UpdateComponentToProjectCommand, OneOf<Unit, ProjectServiceException>>
{
    private readonly IProjectService _projectService;
    private readonly IComponentService _componentService;

    public UpdateComponentToProjectCommandHandler(IComponentService componentService,
                                                  IProjectService projectService)
    {
        _componentService = componentService;
        _projectService = projectService;
    }

    public async Task<OneOf<Unit, ProjectServiceException>> Handle(UpdateComponentToProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(request.ProjectId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Project), request.ProjectId);

            if (request.RequestType == RequestType.add)
            {
                var component = new Details(request.ComponentName, request.ComponentDescription);
                project.AddComponent(component);
            }
            else if (request.RequestType == RequestType.delete)
            {
                var component = await _componentService.GetComponentByNameAsync(request.ComponentName,
                    request.ProjectId,
                    cancellationToken);
                if (component == Guid.Empty)
                {
                    throw new NotFoundException(nameof(Component), request.ComponentName);
                }
                project.RemoveComponent(component);
            }
            await _projectService.UpdateProject(project, cancellationToken);
            return Unit.Value;
        }
        catch (ProjectServiceException ex)
        {
            return ex;
        }
    }
}
