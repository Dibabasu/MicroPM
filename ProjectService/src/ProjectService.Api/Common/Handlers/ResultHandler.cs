using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using ProjectService.Application.Common.Errors;

namespace ProjectService.Api.Common.Handlers;

public static class ResultHandler
{
    public static IActionResult Handle<T>(OneOf<T, ProjectServiceException> result)
    {
        return result.Match<IActionResult>(
            success => new OkObjectResult(success),
            error => new BadRequestObjectResult(error.Message)
        );
    }
}