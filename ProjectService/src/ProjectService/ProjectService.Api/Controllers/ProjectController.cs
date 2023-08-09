using Microsoft.AspNetCore.Mvc;
using ProjectService.Api.Common.Handlers;
using ProjectService.Application.Projects.Commands.CreateProject;
using ProjectService.Application.Projects.Commands.DeleteProject;
using ProjectService.Application.Projects.Commands.UpdateAdminsToProject;
using ProjectService.Application.Projects.Commands.UpdateComponentToProject;
using ProjectService.Application.Projects.Commands.UpdateProject;
using ProjectService.Application.Projects.Commands.UpdateUserGroupsToProject;
using ProjectService.Application.Projects.Commands.UpdateUsersToProject;
using ProjectService.Application.Projects.Commands.UpdateWorkFlowToProject;

namespace ProjectService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectCommand command)
        {
            var result = await Mediator.Send(command);
            return ResultHandler.Handle(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]DeleteProjectCommand command)
        {
            var result = await Mediator.Send(command);
            return ResultHandler.Handle(result);
        }
        [HttpPut("UpdateComponentToProject")]
        public async Task<IActionResult> UpdateComponentToProject([FromBody] UpdateComponentToProjectCommand command)
        {
            var result = await Mediator.Send(command);
            return ResultHandler.Handle(result);
        }
        [HttpPut("UpdateAdminsToProject")]
        public async Task<IActionResult> UpdateAdminsToProject([FromBody] UpdateAdminsToProjectCommand command)
        {
            var result = await Mediator.Send(command);
            return ResultHandler.Handle(result);
        }
        [HttpPut("UpdateProjectDetails")]
        public async Task<IActionResult> UpdateProjectDetails([FromBody] UpdateProjectDetailsCommand command)
        {
            var result = await Mediator.Send(command);
            return ResultHandler.Handle(result);
        }
        [HttpPut("UpdateUserGroupsToProject")]
        public async Task<IActionResult> UpdateUserGroupsToProject([FromBody] UpdateUserGroupsToProjectCommand command)
        {
            var result = await Mediator.Send(command);
            return ResultHandler.Handle(result);
        }
        [HttpPut("UpdateUsersToProject")]
        public async Task<IActionResult> UpdateUsersToProject([FromBody] UpdateUsersToProjectCommand command)
        {
            var result = await Mediator.Send(command);
            return ResultHandler.Handle(result);
        }
        [HttpPut("UpdateWorkFlowToProjectCommand")]
        public async Task<IActionResult> UpdateWorkFlowToProject([FromBody] UpdateWorkFlowToProjectCommand command)
        {
            var result = await Mediator.Send(command);
            return ResultHandler.Handle(result);
        }
    }
}