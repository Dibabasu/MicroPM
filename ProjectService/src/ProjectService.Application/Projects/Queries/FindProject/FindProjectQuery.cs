using MediatR;
using OneOf;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.DTO;
using Mapster;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.Projects.Queries.FindProject;
public class FindProjectQuery : IRequest<OneOf<ProjectDto, ProjectServiceException>>
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
public class FindProjectQueryHandler : IRequestHandler<FindProjectQuery, OneOf<ProjectDto, ProjectServiceException>>
{
    private readonly IProjectService _projectService;

    public FindProjectQueryHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }


    public async Task<OneOf<ProjectDto, ProjectServiceException>> Handle(FindProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await GetProject(request, cancellationToken);

        if (project == null)
        {
            var projectname = string.IsNullOrEmpty(request.Name) ? request.Id.ToString() : request.Name;
            return new NotFoundException(nameof(project), projectname);
        }

        return project.Adapt<ProjectDto>();
    }

    private async Task<Project> GetProject(FindProjectQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            return await _projectService.GetProjectByIdAsync(request.Id.Value, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(request.Name))
        {
            return await _projectService.GetProjectByNameAsync(request.Name, cancellationToken);
        }

        throw new ArgumentException("Either Id or Name must be provided.");
    }
}
