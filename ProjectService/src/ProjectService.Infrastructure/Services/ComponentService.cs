using Microsoft.EntityFrameworkCore;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Entity;
using ProjectService.Infrastructure.Persistence;

namespace ProjectService.Infrastructure.Services;

public class ComponentService : IComponentService
{
    private readonly ProjectServiceDbContext _context;

    public ComponentService(ProjectServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Component> GetComponentByIdAsync(Guid componentId, CancellationToken cancellationToken)
    {
        return await _context.Components.FirstOrDefaultAsync(x => x.Id == componentId, cancellationToken);
    }

    public async Task<Guid> GetComponentByNameAsync(string componentName, Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Components.Where
        (x => x.ProjectId == projectId && x.Details.Name == componentName)
        .Select(x => x.Id)
        .FirstOrDefaultAsync();

    }

    public Task<List<Component>> GetComponentsByNameAsync(IEnumerable<string> enumerable, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Component>> GetComponentsByProjectIdAndNameAsync(Guid projectId, IEnumerable<string> enumerable, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Component>> GetComponentsByProjectIdAndNameAsync(Guid projectId, string componentName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Component>> GetComponentsByProjectIdAndNameAsync(Guid projectId, string componentName, string componentType, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Component>> GetComponentsByProjectIdAndNameAsync(Guid projectId, string componentName, string componentType, string componentStatus, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Component>> GetComponentsByProjectIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> UpdateComponentAsync(Component component, CancellationToken cancellationToken)
    {
        _context.Update(component);
        await _context.SaveChangesAsync(cancellationToken);
        return component.Id;
    }
}