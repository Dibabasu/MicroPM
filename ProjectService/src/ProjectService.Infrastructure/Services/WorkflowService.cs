using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectService.Application.Common.Interfaces;

namespace ProjectService.Infrastructure.Services
{
    public class WorkflowService : IWorkflowService
    {
        public async Task<Guid> GetWorkflowByNameAsync(string Workflowname, CancellationToken cancellationToken)
        {
            return Guid.NewGuid();
        }
    }
}