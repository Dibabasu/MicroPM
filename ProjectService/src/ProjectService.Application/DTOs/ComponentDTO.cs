using Mapster;
using ProjectService.Application.Common.Mappings;
using ProjectService.Domain.Entity;

public class ComponentDto : IMapFrom<ProjectService.Domain.Entity.Component>
{
    public Guid ComponentId { get; set; }
    public Guid ProjectId { get; set; }
    public Details? ComponentDetails{ get; set; }

    public void Mapping(TypeAdapterConfig config)
    {
        config.NewConfig<ProjectService.Domain.Entity.Component, ComponentDto>()
            .Map(dest => dest.ComponentDetails, src => src.ComponentDetails)
            .Map(dest => dest.ProjectId, src => src.ProjectId)
            .Map(dest => dest.ComponentId, src => src.Id);
    }
}