using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;
using ProjectService.Infrastructure.Persistence;

namespace ProjectService.Infrastructure.Services;


public class ProjectServices : IProjectService
{
    private readonly ProjectServiceDbContext _context;

    public ProjectServices(ProjectServiceDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> AddProject(Project project)
    {
        _context.Add(project);
        await _context.SaveChangesAsync();
        return project.ProjectId;
    }
    public async Task<bool> DeleteProjectById(Project project, CancellationToken cancellationToken)
    {
        _context.Remove(project);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Project>? GetProjectByIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId, cancellationToken);
    }

    public async Task<Project>? GetProjectByNameAsync(string projectName, CancellationToken cancellationToken)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.ProjectDetails.Name == projectName, cancellationToken);
    }
    public async Task UpdateProject(Project project, CancellationToken cancellationToken)
    {
        _context.Update(project);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public IQueryable<Project> GetProjects(
    ProjectStatus? status,
    Guid? ownerId,
    DateTime? fromDate,
    DateTime? toDate,
    string? orderBy)
    {
        var query = _context.Projects
        .Include(c => c.Components)
        .Include(pu => pu.ProjectUsers)
        .AsNoTracking();

        query = ApplyFilter(query, () => status.HasValue, p => p.ProjectStatus == status.Value);
        query = ApplyFilter(query, () => ownerId.HasValue, p => p.OwnerId == ownerId.Value);
        query = ApplyFilter(query, () => fromDate.HasValue, p => p.Created >= fromDate.Value);
        query = ApplyFilter(query, () => toDate.HasValue, p => p.Created <= toDate.Value);
        query = ApplyOrder(query, orderBy);
        return query;
    }
    private IQueryable<Project> ApplyOrder(IQueryable<Project> query, string? orderBy)
    {
        var orderMap = new Dictionary<string, Expression<Func<Project, object>>>
        {
            ["projectid"] = p => p.ProjectId,
            ["projectName"] = p => p.ProjectDetails.Name,
            ["created"] = p => p.Created,
            ["projectstatus"] = p => p.ProjectStatus,
        };

        if (string.IsNullOrEmpty(orderBy) || !orderMap.ContainsKey(orderBy.ToLower()))
        {
            orderBy = "projectid"; 
        }

        return query.OrderBy(orderMap[orderBy.ToLower()]);
    }

    private IQueryable<Project> ApplyFilter(IQueryable<Project> query, Func<bool> condition, Expression<Func<Project, bool>> predicate)
    {
        return condition() ? query.Where(predicate) : query;
    }
}