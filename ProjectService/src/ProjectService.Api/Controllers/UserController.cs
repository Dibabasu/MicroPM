using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectService.Application.Common.Interfaces;

namespace ProjectService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ApiControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserGroupService _usergroupService;
    private readonly ICustomClaimService _claimService;
    public UserController(IUserService userService, ICustomClaimService claimService, IUserGroupService usergroupService)
    {
        _userService = userService;
        _claimService = claimService;
        _usergroupService = usergroupService;
    }

    [Authorize]
    [HttpGet("GetUserByuserName")]
    public async Task<IActionResult> GetUserByuserName()
    {
        var username = _claimService.GetUser();
        var users = await _userService.GetUserIdByUserNameAsync(username, cancellationToken: Request.HttpContext.RequestAborted);
        return Ok(users);
    }
    [HttpGet("GetUserByusersGroupName")]
    public async Task<IActionResult> GetUserByusersGroupName(string groupName)
    {
        var users = await _usergroupService.GetUsersByNameAsync(groupName, cancellationToken: Request.HttpContext.RequestAborted);
        return Ok(users);
    }
}

