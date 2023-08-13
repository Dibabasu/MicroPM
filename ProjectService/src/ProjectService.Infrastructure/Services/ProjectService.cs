using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Application.DTO;
using ProjectService.Application.DTOs;
using ProjectService.Application.Projects.Queries.GetProjects;
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
        return project.Id;
    }
    public async Task<bool> DeleteProjectById(Project project, CancellationToken cancellationToken)
    {
        _context.Remove(project);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Project>? GetProjectByIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
    }

    public async Task<Project>? GetProjectByNameAsync(string projectName, CancellationToken cancellationToken)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.Details.Name == projectName, cancellationToken);
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
            .Include(c=>c.Components)
            .Include(pu=>pu.ProjectUsers)
            .AsNoTracking();
           

        if (status.HasValue)
        {
            query = query.Where(p => p.ProjectStatus == status.Value);
        }

        if (ownerId.HasValue)
        {
            query = query.Where(p => p.OwnerId == ownerId.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(p => p.Created >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(p => p.Created <= toDate.Value);
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            switch (orderBy.ToLower())
            {
                case "name":
                    query = query.OrderBy(p => p.Details.Name);
                    break;
                case "createddate":
                    query = query.OrderBy(p => p.Created);
                    break;
                case "status":
                    query = query.OrderBy(p => p.ProjectStatus);
                    break;
            }
        }
        

        return query;
    }

}