using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectService.Application.Common.Mappings
{
    public static class MapperDI
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);

            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
    
}