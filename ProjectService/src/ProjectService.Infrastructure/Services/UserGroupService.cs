using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectService.Application.Common.Interfaces;

namespace ProjectService.Infrastructure.Services
{
    public class UserGroupService : IUserGroupService
    {
        public async Task<List<Guid>> GetUsersByIDAsync(Guid Id, CancellationToken cancellationToken)
        {
           List<Guid> newGuids = new()
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            return newGuids;
        }

        public async Task<List<Guid>> GetUsersByNameAsync(string userGroupNames, CancellationToken cancellationToken)
        {
            ///retrun a list of new guids
            List<Guid> newGuids = new()
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            return newGuids;
        }
    }
}