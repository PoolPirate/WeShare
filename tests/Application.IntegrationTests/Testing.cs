using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System.Reflection;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Common;
using WeShare.Domain.Entities;
using WeShare.Infrastructure.Options;
using WeShare.Infrastructure.Persistence;
using WeShare.WebAPI;

namespace WeShare.Application.IntegrationTests;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot Configuration = null!;
    public static IServiceScopeFactory ScopeFactory = null!;
    private static UserId? CurrentUserId;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        Configuration = builder.Build();

        var startup = new Startup(Configuration);

        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "WeShare.WebAPI"));

        services.AddLogging();

        startup.ConfigureServices(services);

        // Replace service registration for ICurrentUserService
        // Remove existing registration
        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(ICurrentUserService));

        var dispatcherServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IDispatcher));

        if (currentUserServiceDescriptor != null)
        {
            services.Remove(currentUserServiceDescriptor);
        }
        if (dispatcherServiceDescriptor != null)
        {
            services.Remove(dispatcherServiceDescriptor);
        }

        // Register testing version
        services.AddTransient(provider =>
            Mock.Of<ICurrentUserService>(s => s.GetUserId() == CurrentUserId));
        services.AddTransient(provider => 
            Mock.Of<IDispatcher>());
        services.AddTransient(provider =>
            Mock.Of<IPostStorage>(s => s.StoreAsync(It.IsAny<PostId>(), It.IsAny<PostContent>(), It.IsAny<CancellationToken>()) == Task.FromResult(new Entities.PostMetadata(ByteCount.From(10), ByteCount.From(10)))));
        
        var provider = services.BuildServiceProvider();

        await provider.InitializeApplicationAsync(Assembly.GetExecutingAssembly());
        await provider.InitializeApplicationAsync(Assembly.GetAssembly(typeof(ShareDbContext)));
        await provider.InitializeApplicationAsync(Assembly.GetAssembly(typeof(AuthorizationHandler<,>)));

        provider.RunApplication(Assembly.GetExecutingAssembly());
        provider.RunApplication(Assembly.GetAssembly(typeof(ShareDbContext)));
        provider.RunApplication(Assembly.GetAssembly(typeof(AuthorizationHandler<,>)));

        ScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        await EnsureDatabaseAsync();
    }

    private static async Task EnsureDatabaseAsync()
    {
        using var scope = ScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ShareDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = ScopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static void SetCurrentUser(UserId? currentUserId)
    {
        CurrentUserId = currentUserId;
    }

    public static async Task ResetState()
    {
        using var scope = ScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ShareDbContext>()!;
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
        CurrentUserId = null;
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = ScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ShareDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = ScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ShareDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = ScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ShareDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
