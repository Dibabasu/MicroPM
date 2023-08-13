using FluentValidation;

namespace ProjectService.Application.Components.Commands.UpdateComponents;

public class UpdateComponetCommandValidator : AbstractValidator<UpdateComponetCommand>
{
    public UpdateComponetCommandValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
    RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    RuleFor(x=> x.Id).NotEmpty().WithMessage("Component Id is required.");
    }
}