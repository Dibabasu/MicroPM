using System.Diagnostics;
using MediatR;
using Serilog;

namespace ProjectService.Application.Common.Behaviours;
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger _logger;


    public PerformanceBehaviour()
    {
        _timer = new Stopwatch();

         _logger = Log.ForContext<PerformanceBehaviour<TRequest, TResponse>>();

    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            string userName = "Test";

            _logger.Warning("Project Service Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserName} {@Request}",
                requestName, elapsedMilliseconds, userName, request);
        }

        return response;
    }
}