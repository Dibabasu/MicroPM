using Mapster;
using ProjectService.Application.Common.Mappings;
using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;
namespace ProjectService.Application.DTOs;

public class ProjectUserDto : IMapFrom<ProjectUser>
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public UserRole UserRole { get; set; }

    public void Mapping(TypeAdapterConfig config)
    {
        config.NewConfig<ProjectUser, ProjectUserDto>()
            .Map(dest => dest.UserId, src => src.Id)
            .Map(dest => dest.ProjectId, src => src.ProjectId)
            .Map(dest => dest.UserRole, src => src.UserRole);
    }
}