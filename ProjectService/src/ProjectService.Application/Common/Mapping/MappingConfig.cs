using Mapster;
using System.Reflection;
namespace ProjectService.Application.Common.Mappings;
public class MappingConfig
{
    public static void RegisterMappings()
    {
        var config = new TypeAdapterConfig();
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var type in assembly.GetExportedTypes())
        {
            if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            {
                dynamic instance = Activator.CreateInstance(type);
                instance.Mapping(config);
            }
        }

        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
    }
}
