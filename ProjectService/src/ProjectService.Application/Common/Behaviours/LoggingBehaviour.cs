using MediatR.Pipeline;
using ProjectService.Application.Common.Interfaces;
using Serilog;

namespace ProjectService.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> :
    IRequestPreProcessor<TRequest>,
    IRequestExceptionHandler<TRequest, TResponse, Exception>,
    IRequestExceptionAction<TRequest, Exception>
    where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICustomClaimService _claimService;

    public LoggingBehaviour(ICustomClaimService claimService)
    {
        _logger = Log.ForContext<LoggingBehaviour<TRequest, TResponse>>();
        _claimService = claimService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        string userName =  _claimService.GetUser();

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


