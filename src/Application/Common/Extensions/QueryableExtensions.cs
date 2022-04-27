using Microsoft.EntityFrameworkCore;
using WeShare.Application.Actions.Queries;
using WeShare.Domain.Entities;
using WeShare.Domain.Enums;

namespace WeShare.Application.Common.Extensions;
public static class QueryableExtensions
{
    public static Task<Callback?> FindForUserAsync(this IQueryable<Callback> callbacks, UserId userId, CallbackType type)
        => callbacks
            .Where(x => x.OwnerId == userId)
            .Where(x => x.Type == type)
            .SingleOrDefaultAsync();

    public static IQueryable<Share> OrderBy(this IQueryable<Share> shares, ShareOrdering shareOrdering)
        => shareOrdering switch
        {
            ShareOrdering.SubscriberCount => shares
            .OrderBy(x => x.SubscriberCount),
            ShareOrdering.LikeCount => shares
            .OrderBy(x => x.LikeCount),
            _ => throw new InvalidOperationException(),
        };

    public static IQueryable<Share> OrderByDescending(this IQueryable<Share> shares, ShareOrdering shareOrdering)
    => shareOrdering switch
    {
        ShareOrdering.SubscriberCount => shares
        .OrderByDescending(x => x.SubscriberCount),
        ShareOrdering.LikeCount => shares
        .OrderByDescending(x => x.LikeCount),
        _ => throw new InvalidOperationException(),
    };
}
