using Mapster;

namespace ProjectService.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(TypeAdapterConfig config) => config.Adapt(typeof(T), GetType());
}