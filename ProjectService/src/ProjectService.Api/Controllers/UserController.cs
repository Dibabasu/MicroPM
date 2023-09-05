using Microsoft.AspNetCore.Mvc;
using ProjectService.Application.Common.Interfaces;

namespace ProjectService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ApiControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserByuserName([FromQuery] string username)
    {
        var users = await _userService.GetUserIdByUserNameAsync(username,cancellationToken: Request.HttpContext.RequestAborted);
        return Ok(users);
    }
}

    