using MediatR.Pipeline;
using Serilog;

namespace ProjectService.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> :
    IRequestPreProcessor<TRequest>,
    IRequestExceptionHandler<TRequest, TResponse, Exception>,
    IRequestExceptionAction<TRequest, Exception>
    where TRequest : notnull
{
    private readonly ILogger _logger;

    public LoggingBehaviour()
    {
        _logger = Log.ForContext<LoggingBehaviour<TRequest, TResponse>>();
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        string userName = string.Empty; // Consider fetching the actual username if possible

        _logger.Information("Project Service Request: {Name}  {@UserName} {@Request}",
            requestName, userName, request);
    }

    public async Task Handle(TRequest request, Exception exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        _logger.Error(exception, "Unhandled Exception for Request {Name} {@Request}",
            typeof(TRequest).Name, request);
    }

    public async Task ExceptionHandled(TRequest request, Exception exception)
    {
        _logger.Error(exception,"Exception Handled for Request {Name} {@Request}",
            typeof(TRequest).Name, request);
    }

    public async Task Execute(TRequest request, Exception exception, CancellationToken cancellationToken)
    {
        _logger.Error(exception,"Exception Handled for Request {Name} {@Request}",
        typeof(TRequest).Name, request);
    }
}


