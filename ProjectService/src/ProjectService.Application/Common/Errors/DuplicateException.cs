namespace ProjectService.Application.Common.Errors;

public class DuplicateException : ProjectServiceException
{
    public DuplicateException(string message) : base($"Entity \"{message}\"Already Exists") { }

    public DuplicateException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.") { }
    
}