using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Infrastructure.HttpClients;

namespace ProjectService.Infrastructure.Services
{
    public class UserGroupService : IUserGroupService
    {
        private readonly UserGroupServiceClient _usergroupServiceClient;

        public UserGroupService(UserGroupServiceClient userGroupServiceClient)
        {
            _usergroupServiceClient = userGroupServiceClient;
        }
        public async Task<List<string>> GetUsersByNameAsync(string userGroupNames, CancellationToken cancellationToken)
        {
            var users = await _usergroupServiceClient.GetUsersByGroup(userGroupNames, cancellationToken)
            ?? throw new NotFoundException(userGroupNames);

            return users.Select(user => user.UserName).ToList();
        }
    }
}