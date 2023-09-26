using ProjectService.Domain.Entity;

namespace ProjectService.Application.Common.Interfaces;

public interface IUserGroupService
{
    public Task<List<Guid>> GetUsersByNameAsync(string userGroupNames,CancellationToken cancellationToken);
}