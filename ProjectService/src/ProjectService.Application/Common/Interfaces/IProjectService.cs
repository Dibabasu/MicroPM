using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.Common.Interfaces;

public interface IProjectService
{
    public  Task<Guid> AddProject(Project project);
    public Task<Project>? GetProjectByNameAsync(string projectName,CancellationToken cancellationToken);

    public Task<Project>? GetProjectByIdAsync(Guid projectId,CancellationToken cancellationToken);

    public IQueryable<Project> GetProjects(
    ProjectStatus? Status,
    Guid? OwnerId,
    DateTime? FromDate,
    DateTime? ToDate,
    string? OrderBy);

    public Task UpdateProject(Project project,CancellationToken cancellationToken);
    public Task<bool> DeleteProjectById(Project project,CancellationToken cancellationToken);
   
}