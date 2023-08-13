using MediatR;
using OneOf;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommand : IRequest<OneOf<Unit, ProjectServiceException>>
{
    public Guid ProjectId { get; set; }
}
public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, OneOf<Unit, ProjectServiceException>>
{
    private readonly IProjectService _projectService;

    public DeleteProjectCommandHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<OneOf<Unit, ProjectServiceException>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
        var project = await _projectService.GetProjectByIdAsync(request.ProjectId, cancellationToken)
                      ?? throw new NotFoundException(nameof(Project), request.ProjectId);
        project.ChangeStatus(ProjectStatus.deleted);
        return Unit.Value;
        }
        catch(ProjectServiceException ex)
        {
            return ex;
        }
    }
}