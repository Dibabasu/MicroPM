using ProjectService.Domain.Entity;

namespace ProjectService.Application.Common.Interfaces;

public interface IUserGroupService
{
    public Task<List<Guid>> GetUsersByNameAsync(string userGroupNames,CancellationToken cancellationToken);
    public Task<List<Guid>> GetUsersByIDAsync(Guid Id,CancellationToken cancellationToken);

}