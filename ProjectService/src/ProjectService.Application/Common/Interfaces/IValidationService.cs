namespace ProjectService.Application.Common.Interfaces;

public interface IValidationService
{
    public ValueTask<Guid> ValidateUser(string ownerName);
    public ValueTask<Guid> ValidateWorkflow(string workflowName);
    public ValueTask<List<Guid>> ValidateUserGroup(string userGroupName);
}