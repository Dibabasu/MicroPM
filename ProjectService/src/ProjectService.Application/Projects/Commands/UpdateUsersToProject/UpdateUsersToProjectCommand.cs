using MediatR;
using OneOf;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.Projects.Commands.UpdateUsersToProject;

public class UpdateUsersToProjectCommand : IRequest<OneOf<Unit, ProjectServiceException>>
{
    public Guid ProjectId { get; set; }
    public List<string>? UserNames { get; set; }
    public RequestType RequestType { get; set; }
}
public class UpdateUsersToProjectCommandHandler : IRequestHandler<UpdateUsersToProjectCommand, OneOf<Unit, ProjectServiceException>>
{
    private readonly IProjectService _projectService;
    private readonly IUserService _userService;

    public UpdateUsersToProjectCommandHandler(IProjectService projectService, IUserService userService)
    {
        _projectService = projectService;
        _userService = userService;
    }

    public async Task<OneOf<Unit, ProjectServiceException>> Handle(UpdateUsersToProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(request.ProjectId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Project), request.ProjectId);
            if (request.UserNames != null)
            {
                foreach (var userName in request.UserNames)
                {
                    Guid userId = await _userService.GetUserIdByUserNameAsync(userName, cancellationToken);
                    if (userId == Guid.Empty)
                    {
                        throw new NotFoundException("userName", userName);
                    }
                    if (request.RequestType == RequestType.add)
                    {
                        project.AddUser(userId, UserRole.user
                        );
                    }
                    else
                    {
                        project.RemoveUser(userId);
                    }
                }
            }
            await _projectService.UpdateProject(project, cancellationToken);
            return Unit.Value;
        }
        catch (ProjectServiceException ex)
        {

            return ex;
        }
    }
}