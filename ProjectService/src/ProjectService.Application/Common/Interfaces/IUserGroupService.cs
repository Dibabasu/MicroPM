namespace ProjectService.Application.Common.Interfaces;

public interface IUserGroupService
{
    public Task<List<string>> GetUsersByNameAsync(string userGroupNames,CancellationToken cancellationToken);
}