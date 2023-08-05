using Microsoft.AspNetCore.Mvc;
using ProjectService.Application.Projects.Commands.CreateProject;

namespace ProjectService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateProjectCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}