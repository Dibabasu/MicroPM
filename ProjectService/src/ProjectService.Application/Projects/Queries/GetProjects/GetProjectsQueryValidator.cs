using FluentValidation;

namespace ProjectService.Application.Projects.Queries.GetProjects;

public class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
{
    public GetProjectsQueryValidator()
    {
        RuleFor(x => x.PageSize)
            .LessThanOrEqualTo(50)
            .WithMessage("PageSize must not be greater than 50.");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize must be greater than 1.");
    }
}