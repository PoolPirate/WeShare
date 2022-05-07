using WeShare.Application.Actions.Queries;
using WeShare.Domain.Entities;

namespace WeShare.Application.Common.Extensions;
public static class QueryableExtensions
{
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

    public static IQueryable<Post> OrderBy(this IQueryable<Post> posts, PostOrdering postOrdering)
        => postOrdering switch
        {
            PostOrdering.CreatedAtDesc => posts
            .OrderByDescending(x => x.CreatedAt),
            PostOrdering.PayloadSize => posts
            .OrderBy(x => x.PayloadSize),
            PostOrdering.PayloadSizeDec => posts
            .OrderByDescending(x => x.PayloadSize),
            _ => throw new InvalidOperationException(),
        };
}
