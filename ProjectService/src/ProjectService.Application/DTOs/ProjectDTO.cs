using Mapster;
using ProjectService.Application.Common.Mappings;
using ProjectService.Application.DTOs;
using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;
namespace ProjectService.Application.DTO;
public class ProjectDto : IMapFrom<Project>
{
    public Guid ProjectId { get; set; }
    public DetailsDto? ProjectDetails { get; set; }
    public Guid OwnerId { get; set; }
    public ICollection<ComponentDto>? Components { get; set; }
    public Guid WorkflowId { get; set; }
    public ICollection<ProjectUserDto>? ProjectUsers { get; set; }
    public ProjectStatus ProjectStatus { get; set; }

    public void Mapping(TypeAdapterConfig config)
    {
        config.NewConfig<Project, ProjectDto>()
            .Map(dest => dest.ProjectId, src => src.Id)
            .Map(dest => dest.ProjectDetails, src => src.ProjectDetails)
            .Map(dest => dest.OwnerId, src => src.OwnerId)
            .Map(dest => dest.Components, src => src.Components)
            .Map(dest => dest.WorkflowId, src => src.WorkflowId)
            .Map(dest => dest.ProjectUsers, src => src.ProjectUsers)
            .Map(dest => dest.ProjectStatus, src => src.ProjectStatus);
    }
}
