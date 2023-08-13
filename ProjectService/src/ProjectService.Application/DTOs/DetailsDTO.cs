using Mapster;
using ProjectService.Application.Common.Mappings;
using ProjectService.Domain.Entity;

namespace ProjectService.Application.DTOs;

public class DetailsDto : IMapFrom<Details>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public void Mapping(TypeAdapterConfig config)
    {
        config.NewConfig<Details, DetailsDto>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);
    }
}