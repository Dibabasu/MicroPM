using System.Net;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using ProjectService.Application.Common.Services;
using ProjectService.Application.Projects.Queries.FindProject;
using ProjectService.Domain.Common;

namespace ProjectService.Api.Filters;
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class ETagFilter : ActionFilterAttribute, IAsyncActionFilter
{
    private readonly IMediator _mediator;
    private readonly ETagService _etagService;
    private readonly string[] _supportedVerbs = new[] { HttpMethod.Get.Method, HttpMethod.Post.Method, HttpMethod.Put.Method };

    public ETagFilter(IMediator mediator, ETagService etagService)
    {
        _mediator = mediator;
        _etagService = etagService;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
    {
        var request = executingContext.HttpContext.Request;

        if (!request.Method.Equals(HttpMethod.Get) && request.Headers.ContainsKey(HeaderNames.IfMatch))
        {
            if (!await ValidateETagForMidAirEditsCollision(executingContext))
            {
                return;
            }
        }

        var executedContext = await next();

        var response = executedContext.HttpContext.Response;

        if (_supportedVerbs.Contains(request.Method) && response.StatusCode == (int)HttpStatusCode.OK)
        {
            ValidateETagForResponseCaching(executedContext);
        }
    }

    private void ValidateETagForResponseCaching(ActionExecutedContext executedContext)
    {
        if (executedContext.Result == null)
        {
            return;
        }

        var request = executedContext.HttpContext.Request;
        var response = executedContext.HttpContext.Response;

        var result = (BaseEntity)(executedContext.Result as ObjectResult).Value;

        // generate ETag from LastModified property
        //var etag = GenerateEtagFromLastModified(result.LastModified);

        // generates ETag from the entire response Content
        var etag = GenerateEtagFromResponseBodyWithHash(result);

        if (request.Headers.ContainsKey(HeaderNames.IfNoneMatch))
        {
            // fetch etag from the incoming request header
            var incomingEtag = request.Headers[HeaderNames.IfNoneMatch].ToString();

            // if both the etags are equal
            // raise a 304 Not Modified Response
            if (incomingEtag.Equals(etag))
            {
                executedContext.Result = new StatusCodeResult((int)HttpStatusCode.NotModified);
            }
        }

        // add ETag response header 
        response.Headers.Add(HeaderNames.ETag, new[] { etag });
    }

    private async Task<bool> ValidateETagForMidAirEditsCollision(ActionExecutingContext executingContext)
    {
        var request = executingContext.HttpContext.Request;

        // detect collision only for an Edit (PUT) request
        if (request.Method == HttpMethod.Put.Method)
        {
            var incomingETag = request.Headers[HeaderNames.IfMatch];

            object itemId;
            if (request.RouteValues.TryGetValue("id", out itemId))
            {
                var command = new FindProjectQuery { Id = Guid.Parse(itemId.ToString()) };
                var result = await _mediator.Send(command);

                if (result.IsT0)
                {
                    // generates ETag from the entire response Content
                    var etag = _etagService.ComputeWithHashFunction(result.AsT0);

                    // if both the etags are not equal
                    // the data has already changed on the server side
                    // mid-air collision
                    // respond with a 412
                    if (!incomingETag.Equals(etag))
                    {
                        executingContext.Result = new StatusCodeResult((int)HttpStatusCode.PreconditionFailed); return false;
                    }
                }
            }
        }

        return true;
    }

    private string GenerateEtagFromResponseBodyWithHash(BaseEntity tmpSource)
    {
        return _etagService.ComputeWithHashFunction(tmpSource);
    }

    private string GenerateEtagFromLastModified(DateTime lastModifiedDateTime)
    {
        var strDateTime = lastModifiedDateTime.ToString();
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(strDateTime));
    }
}
