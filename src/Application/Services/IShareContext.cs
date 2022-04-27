using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WeShare.Application.Common;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;

public interface IShareContext
{
    DbSet<User> Users { get; }
    DbSet<Share> Shares { get; }
    DbSet<Like> Likes { get; }
    DbSet<Callback> Callbacks { get; }
    DbSet<Post> Posts { get; }
    DbSet<Subscription> Subscriptions { get; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task<DbSaveResult> SaveChangesAsync(DbStatus allowedStatuses = DbStatus.Success,
        IDbContextTransaction? transaction = null, CancellationToken cancellationToken = default);
}
