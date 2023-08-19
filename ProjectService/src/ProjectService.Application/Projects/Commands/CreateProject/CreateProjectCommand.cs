using MediatR;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Entity;
using ProjectService.Application.Common.Errors;
using OneOf;
using ProjectService.Domain.Common;
using Serilog;
using ProjectService.Application.DTOs;

namespace ProjectService.Application.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<OneOf<Guid, ProjectServiceException>>
{
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectDescription { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public List<CreateComponentDTO>? Components { get; set; }
    public List<string>? UserGroup { get; set; }
    public List<string>? Users { get; set; }
    public string Workflow { get; set; } = string.Empty;
    public List<string>? Admin { get; set; }
}


public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, OneOf<Guid, ProjectServiceException>>
{
    private readonly IValidationService _validationService;
    private readonly IProjectService _projectService;
    private readonly ILogger _logger;

    public CreateProjectCommandHandler(IValidationService validationService,
                                       IProjectService projectService)
    {
        _validationService = validationService;
        _projectService = projectService;
        _logger = Log.ForContext<CreateProjectCommandHandler>();
    }

    public async Task<OneOf<Guid, ProjectServiceException>> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var owner = await _validationService.ValidateUser(request.Owner);
            Project project = new(
                projectDetails: new Details(request.ProjectName, request.ProjectDescription),
                ownerId: owner,
                workflowId: await _validationService.ValidateWorkflow(request.Workflow));
            if (request.Admin != null)
            {
                foreach (var admin in request.Admin)
                {
                    var user = await _validationService.ValidateUser(admin);
                    project.AddAdmin(user);
                }
            }
            if (request.Users != null)
            {
                foreach (var user in request.Users)
                {
                    var userToAdd = await _validationService.ValidateUser(user);
                    project.AddUser(userToAdd, UserRole.user);
                }
            }
            if (request.UserGroup != null)
            {
                foreach (var userGroup in request.UserGroup)
                {
                    var listofUser = await _validationService.ValidateUserGroup(userGroup);
                    if (listofUser != null)
                    {
                        project.AddUsers(listofUser);
                    }

                }
            }
            if (request.Components != null)
            {
                foreach (var component in request.Components)
                {
                    project.AddComponent(new Details(component.ComponentName,
                        component.ComponentDescription));
                }

            }
            await _projectService.AddProject(project);
            return project.Id;
        }
        catch(ProjectServiceException ex)
        {
            return ex;
        }
        
    }
}