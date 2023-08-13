using MediatR;
using OneOf;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.Projects.Commands.UpdateProject;

public class UpdateProjectDetailsCommand : IRequest<OneOf<Unit, ProjectServiceException>>
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
public class UpdateProjectDetailsCommandHandler : IRequestHandler<UpdateProjectDetailsCommand, OneOf<Unit, ProjectServiceException>>
{
    private readonly IProjectService _projectService;

    public UpdateProjectDetailsCommandHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<OneOf<Unit, ProjectServiceException>> Handle(UpdateProjectDetailsCommand request, CancellationToken cancellationToken)
    {
        try
        {


            var project = await _projectService.GetProjectByIdAsync(request.ProjectId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Project), request.ProjectId);

            project.UpdateDetails(new Details(
                    request.Name,
                    request.Description));

            await _projectService.UpdateProject(project, cancellationToken);

            return Unit.Value;
        }
        catch (ProjectServiceException ex)
        {

            return ex;
        }
    }
}