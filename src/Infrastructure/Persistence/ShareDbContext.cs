using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using WeShare.Application.Common;
using WeShare.Application.Services;
using WeShare.Domain.Common;
using WeShare.Domain.Entities;
using WeShare.Infrastructure.Persistence.Concurrency;

namespace WeShare.Infrastructure.Persistence;

public class ShareDbContext : MergingDbContext, IShareContext
{
    private readonly IDomainEventService DomainEventService;

    public DbSet<User> Users { get; set; }
    public DbSet<Share> Shares { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Callback> Callbacks { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    public ShareDbContext(
        DbContextOptions<ShareDbContext> options,
        Random random,
        PropertyMerger propertyMerger,
        IDomainEventService domainEventService)
        : base(options, random, propertyMerger)
    {
        DomainEventService = domainEventService;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.IgnoreAny<IList<DomainEvent>>();

        configurationBuilder.Properties<UserId>().HaveConversion<UserId.EfCoreValueConverter>();
        configurationBuilder.Properties<ShareId>().HaveConversion<ShareId.EfCoreValueConverter>();
        configurationBuilder.Properties<CallbackId>().HaveConversion<CallbackId.EfCoreValueConverter>();
        configurationBuilder.Properties<PostId>().HaveConversion<PostId.EfCoreValueConverter>();
        configurationBuilder.Properties<SubscriptionId>().HaveConversion<SubscriptionId.EfCoreValueConverter>();

        configurationBuilder.Properties<Username>().HaveConversion<Username.EfCoreValueConverter>();
        configurationBuilder.Properties<Nickname>().HaveConversion<Nickname.EfCoreValueConverter>();

        configurationBuilder.Properties<CallbackSecret>().HaveConversion<CallbackSecret.EfCoreValueConverter>();

        configurationBuilder.Properties<ShareSecret>().HaveConversion<ShareSecret.EfCoreValueConverter>();
        configurationBuilder.Properties<Sharename>().HaveConversion<Sharename.EfCoreValueConverter>();

        configurationBuilder.Properties<ByteCount>().HaveConversion<ByteCount.EfCoreValueConverter>();

        base.ConfigureConventions(configurationBuilder);
    }

    public override async Task<DbSaveResult> SaveChangesAsync(DbStatus allowedStatuses = DbStatus.Success,
        IDbContextTransaction? transaction = null, CancellationToken cancellationToken = new CancellationToken())
    {
        EnsureTransactionIsUsed(transaction);

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    break;
            }
        }

        var domainEvents = GetUnpublishedDomainEvents();

        if (Database.CurrentTransaction is null && domainEvents.Length != 0)
        {
            transaction = await Database.BeginTransactionAsync(cancellationToken);
        }

        try
        {
            var result = await base.SaveChangesAsync(allowedStatuses, cancellationToken: cancellationToken);

            if (result.Status == DbStatus.Success)
            {
                await DispatchEvents(domainEvents);

                if (transaction is not null)
                {
                    await transaction.CommitAsync(cancellationToken);
                }
            }

            return result;
        }
        catch (Exception)
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync(cancellationToken);
            }
            throw;
        }
        finally
        {
            if (transaction is not null)
            {
                await transaction.DisposeAsync();
            }
        }
    }

    private void EnsureTransactionIsUsed(IDbContextTransaction? transaction)
    {
        if (transaction != Database.CurrentTransaction)
        {
            throw new InvalidOperationException("This context does not use the given transaction!");
        }
    }

    private DomainEvent[] GetUnpublishedDomainEvents()
        => ChangeTracker.Entries<IHasDomainEvents>()
            .SelectMany(x => x.Entity.DomainEvents)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var domainEvent in events)
        {
            domainEvent.IsPublished = true;
            await DomainEventService.Publish(domainEvent);
        }
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        => Database.BeginTransactionAsync(cancellationToken);
}
