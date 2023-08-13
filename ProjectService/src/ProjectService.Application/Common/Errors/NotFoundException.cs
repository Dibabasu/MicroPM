namespace ProjectService.Application.Common.Errors;

public class NotFoundException : ProjectServiceException
{
    public NotFoundException(string? message) : base($"Entity \"{message}\" was not found.") { }

    public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.") { }
}