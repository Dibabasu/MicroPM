using MediatR;
using OneOf;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.Components.Commands.UpdateComponents;

public class UpdateComponetCommand : IRequest<OneOf<Guid, ProjectServiceException>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
public class UpdateComponetCommandHandler : IRequestHandler<UpdateComponetCommand, OneOf<Guid, ProjectServiceException>>
{
    private readonly IComponentService _componentService;

    public UpdateComponetCommandHandler(IComponentService componentService)
    {
        _componentService = componentService;
    }

    public async Task<OneOf<Guid, ProjectServiceException>> Handle(UpdateComponetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var component = await _componentService.GetComponentByIdAsync(request.Id, cancellationToken);
            if (component == null)
            {
                return new NotFoundException(nameof(component), request.Id);
            }
            component.UpdateDetails(new Details(request.Name, request.Description),component.ProjectId,request.Id);
            var result = await _componentService.UpdateComponentAsync(component, cancellationToken);
            return result;
        }
        catch (ProjectServiceException ex)
        {
            return ex;
        }
    }
}


