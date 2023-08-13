namespace ProjectService.Domain.Common;
public enum ProjectStatus
{
    draft = 0,
    approved = 1,
    rejected = -1,
    deleted = -2
}
public enum UserRole
{
    user = 0,
    admin = 1
}