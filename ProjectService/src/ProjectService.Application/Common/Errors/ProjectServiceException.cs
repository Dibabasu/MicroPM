namespace ProjectService.Application.Common.Errors;

public abstract class ProjectServiceException : Exception
{
    protected ProjectServiceException(string message) : base(message)
    {
    }
}