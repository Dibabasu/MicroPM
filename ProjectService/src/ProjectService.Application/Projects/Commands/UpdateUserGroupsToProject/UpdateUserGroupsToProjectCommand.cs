using MediatR;
using OneOf;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.Projects.Commands.UpdateUserGroupsToProject;

public class UpdateUserGroupsToProjectCommand : IRequest<OneOf<Unit, ProjectServiceException>>
{
    public Guid ProjectId { get; set; }
    public List<string>? UserGroupNames { get; set; }
    public RequestType RequestType { get; set; }
}
public class UpdateUserGroupsToProjectCommandHandler : IRequestHandler<UpdateUserGroupsToProjectCommand, OneOf<Unit, ProjectServiceException>>
{
    private readonly IProjectService _projectService;
    private readonly IUserGroupService _groupService;

    public UpdateUserGroupsToProjectCommandHandler(IProjectService projectService, IUserGroupService groupService)
    {
        _projectService = projectService;
        _groupService = groupService;
    }

    public async Task<OneOf<Unit, ProjectServiceException>> Handle(UpdateUserGroupsToProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(request.ProjectId, cancellationToken)
                         ?? throw new NotFoundException(nameof(Project), request.ProjectId);
            if (request.UserGroupNames != null)
            {
                foreach (var UserGroupNames in request.UserGroupNames)
                {
                    List<Guid> UserIds = await _groupService.GetUsersByNameAsync(UserGroupNames,cancellationToken);
                    if (request.RequestType == RequestType.add)
                    {
                        project.AddUsers(UserIds);
                    }
                    else
                    {
                        project.RemoveUsers(UserIds);
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