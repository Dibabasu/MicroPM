namespace ProjectService.Application.Common.Errors;

public class EmptyOrNullException : ProjectServiceException
{
    public EmptyOrNullException(string name) : base($"Entity \"{name}\"was null or empty.") { }
}