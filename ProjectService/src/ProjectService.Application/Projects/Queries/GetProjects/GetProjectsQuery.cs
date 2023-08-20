using Mapster;
using MediatR;
using OneOf;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Mappings;
using ProjectService.Application.Common.Models;
using ProjectService.Application.DTO;
using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.Projects.Queries.GetProjects;

public class GetProjectsQuery : IRequest<OneOf<PagedList<ProjectDto>, ProjectServiceException>>
{
    public ProjectStatus? Status { get; set; }
    public Guid? OwnerId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string OrderBy { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, OneOf<PagedList<ProjectDto>, ProjectServiceException>>
{
    private readonly IProjectService _projectService;

    public GetProjectsQueryHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<OneOf<PagedList<ProjectDto>, ProjectServiceException>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projectsQuery = _projectService.GetProjects(
                                            request.Status,
                                            request.OwnerId,
                                            request.FromDate,
                                            request.ToDate,
                                            request.OrderBy);

        if (!projectsQuery.Any())
        {
            throw new EmptyOrNullException("No projects found");
        }

        projectsQuery = projectsQuery.OrderBy(p => p.Id);

        var projectDtos = await projectsQuery.ToPagedListAsync<Project, ProjectDto>(request.PageNumber, request.PageSize);

        return projectDtos;
    }
}