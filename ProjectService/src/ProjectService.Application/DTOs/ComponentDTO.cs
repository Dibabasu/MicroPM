using Mapster;
using ProjectService.Application.Common.Mappings;
using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.DTOs;

public class ComponentDto : AuditableEntity, IMapFrom<Component>
{
    public Guid ComponentId { get; set; }
    public Guid ProjectId { get; set; }
    public Details? ComponentDetails { get; set; }

    public void Mapping(TypeAdapterConfig config)
    {
        config.NewConfig<Component, ComponentDto>()
            .Map(dest => dest.ComponentDetails, src => src.ComponentDetails)
            .Map(dest => dest.ProjectId, src => src.ProjectId)
            .Map(dest => dest.ComponentId, src => src.Id);
    }
}