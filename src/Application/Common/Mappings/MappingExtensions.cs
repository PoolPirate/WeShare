using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Models;

namespace WeShare.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable,
        int pageNumber, int pageSize,
        CancellationToken cancellationToken = default) => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize, cancellationToken);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration,
        CancellationToken cancellationToken) => queryable.ProjectTo<TDestination>(configuration).ToListAsync(cancellationToken);
}
