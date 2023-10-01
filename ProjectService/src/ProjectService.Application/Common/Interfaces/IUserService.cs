namespace ProjectService.Application.Common.Interfaces;

public interface IUserService
{
    public Task<string> GetUserIdByUserNameAsync(string username,CancellationToken cancellationToken);
}