using ProjectService.Domain.Entity;

namespace ProjectService.Application.Common.Interfaces;

public interface IUserService
{
    public Task<Guid> GetUserIdByUserNameAsync(string username,CancellationToken cancellationToken);
}