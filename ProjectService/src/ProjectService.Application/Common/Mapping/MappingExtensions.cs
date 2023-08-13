using Mapster;
using ProjectService.Application.Common.Models;

namespace ProjectService.Application.Common.Mappings;
public static class MappingExtensions
{
    public static async Task<PagedList<TDestination>> ToPagedListAsync<TSource, TDestination>(this IQueryable<TSource> queryable,  int pageNumber, int pageSize) where TDestination : class
    {
        var projected = queryable.ProjectToType<TDestination>();
        return await PagedList<TDestination>.CreateAsync(projected, pageNumber, pageSize);
    }
}
