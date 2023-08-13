using ProjectService.Domain.Entity;

namespace ProjectService.Application.Common.Interfaces;

public interface IComponentService
{
  public Task<Guid> GetComponentByNameAsync(
      string componentName, Guid projectId,
       CancellationToken cancellationToken);
  public Task<List<Component>> GetComponentsByNameAsync(
      IEnumerable<string> enumerable, CancellationToken cancellationToken);

  public Task<Guid> UpdateComponentAsync(Component component, CancellationToken cancellationToken);

  public Task<List<Component>> GetComponentsByProjectIdAsync(
      Guid projectId, CancellationToken cancellationToken);

  public Task<Component> GetComponentByIdAsync(
      Guid componentId, CancellationToken cancellationToken);

  public Task<List<Component>> GetComponentsByProjectIdAndNameAsync(
      Guid projectId, IEnumerable<string> enumerable,
      CancellationToken cancellationToken);

  public Task<List<Component>> GetComponentsByProjectIdAndNameAsync(
      Guid projectId, string componentName,
      CancellationToken cancellationToken);

  public Task<List<Component>> GetComponentsByProjectIdAndNameAsync(
      Guid projectId, string componentName,
      string componentType, CancellationToken cancellationToken);

  public Task<List<Component>> GetComponentsByProjectIdAndNameAsync(
      Guid projectId, string componentName,
      string componentType, string componentStatus,
      CancellationToken cancellationToken);
}