using Mapster;
using ProjectService.Application.Common.Mappings;

public class ComponentDto : IMapFrom<ProjectService.Domain.Entity.Component>
{
    public Guid ComponentId { get; set; }
    public Guid ProjectId { get; set; }
    public string ComponentName { get; set; }
    public string ComponentDescription { get; set; } 

    public void Mapping(TypeAdapterConfig config)
    {
        config.NewConfig<ProjectService.Domain.Entity.Component, ComponentDto>()
            .Map(dest => dest.ComponentName, src => src.Details.Name)
            .Map(dest => dest.ComponentDescription, src => src.Details.Description)
            .Map(dest => dest.ProjectId, src => src.ProjectId)
            .Map(dest => dest.ComponentId, src => src.Id);
    }
}