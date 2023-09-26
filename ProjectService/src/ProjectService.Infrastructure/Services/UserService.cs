using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Infrastructure.HttpClients;

namespace ProjectService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserServiceClient _userServiceClient;

        public UserService(UserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        public async Task<Guid> GetUserIdByUserNameAsync(string username, CancellationToken cancellationToken)
        {
            var user = await _userServiceClient.GetUser(username, cancellationToken);
            if (String.IsNullOrEmpty(user.Sid))
            {
                throw new NotFoundException(username);
            }
            return Guid.Parse(user.Sid);
        }
    }
}