using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectService.Application.Common.Interfaces;

namespace ProjectService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        public async Task<Guid> GetUserIdByUserNameAsync(string username, CancellationToken cancellationToken)
        {
            return (Guid.NewGuid());
        }
    }
}