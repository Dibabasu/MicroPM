using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectService.Application.Common.Interfaces;

namespace ProjectService.Infrastructure.Services
{
    public class UserGroupService : IUserGroupService
    {
        public Task<List<Guid>> GetUsersByIDAsync(Guid Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Guid>> GetUsersByNameAsync(string userGroupNames, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}