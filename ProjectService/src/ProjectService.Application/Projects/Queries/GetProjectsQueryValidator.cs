using FluentValidation;

namespace ProjectService.Application.Projects.Queries.GetProjects;

public class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
{
    public GetProjectsQueryValidator()
    {
        RuleFor(x => x.PageSize)
            .LessThanOrEqualTo(50)
            .WithMessage("PageSize must not be greater than 50.");
    }
}