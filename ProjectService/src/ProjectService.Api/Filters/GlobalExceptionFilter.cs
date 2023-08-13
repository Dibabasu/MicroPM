using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectService.Application.Common.Errors;

namespace ProjectService.Api.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = context.HttpContext.Request.Path,
            
        };

        switch (context.Exception)
        {
            case ValidationException validationException:
                problemDetails.Title = "Validation error";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = string.Join(", ", validationException.Errors.Select(e =>
                {
                    return e.Value;
                }));
                break;

            case NotFoundException notFoundException:
                problemDetails.Title = "Resource not found";
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Detail = notFoundException.Message;
                break;

            default:
                problemDetails.Title = "An unexpected error occurred!";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Detail = context.Exception.Message;
                break;
        }

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
        context.ExceptionHandled = true;
    }
}
