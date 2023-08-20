using Mapster;
using System.Reflection;

namespace ProjectService.Application.Common.Mappings;
public static class MappingConfig
{
    public static void RegisterMappings()
    {
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var type in assembly.GetExportedTypes())
        {
            if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            {
                dynamic instance = Activator.CreateInstance(type);
                instance.Mapping(TypeAdapterConfig.GlobalSettings);
            }
        }

        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
    }
}
