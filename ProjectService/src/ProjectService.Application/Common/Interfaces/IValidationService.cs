namespace ProjectService.Application.Common.Interfaces;

public interface IValidationService
{
    public ValueTask<string> ValidateUser(string ownerName);
    public ValueTask<Guid> ValidateWorkflow(string workflowName);
    public ValueTask<List<string>> ValidateUserGroup(string userGroupName);
}