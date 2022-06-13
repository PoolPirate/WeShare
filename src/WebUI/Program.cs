using AutoMapper;
using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WeShare.Application.Common.Security;
using WeShare.Infrastructure.Persistence;
using WeShare.WebAPI.Options;

namespace WeShare.WebAPI;

public sealed class Program
{
    /// <summary>
    /// The time in milliseconds after which to force a shutdown.
    /// </summary>
    private const int ShutdownTimeout = 5000;

    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    private static readonly Assembly InfrastructureAssembly = Assembly.GetAssembly(typeof(ShareDbContext))
            ?? throw new InvalidOperationException("Could not load Infrastructure Assembly!");
    private static readonly Assembly ApplicationAssembly = Assembly.GetAssembly(typeof(AuthorizationHandler<,>))
            ?? throw new InvalidOperationException("Could not load Application Assembly!");

    private static IHost? Application;
    private static IServiceProvider? Provider;
    private static ILogger? Logger;

    public static async Task Main(string[] args)
    {
        Application = CreateApplication(args);
        Provider = Application.Services;
        Logger = Provider.GetRequiredService<ILogger<Program>>();

        EnsureValidMapperConfiguration();
        await MigrateDatabaseAsync();

        await Provider.InitializeApplicationAsync(Assembly);
        await Provider.InitializeApplicationAsync(InfrastructureAssembly);
        await Provider.InitializeApplicationAsync(ApplicationAssembly);

        Provider.RunApplication(Assembly);
        Provider.RunApplication(InfrastructureAssembly);
        Provider.RunApplication(ApplicationAssembly);

        await Application.RunAsync();
    }

    private static IHost CreateApplication(string[] args)
    {
        var webApp = WebApplication.CreateBuilder(args);

        webApp.Logging.AddConsole();
        webApp.Configuration.AddJsonFile("appsettings.json", false);
        webApp.WebHost.ConfigureKestrel((context, options) =>
        {
            var bindingOptions = options.ApplicationServices.GetRequiredService<BindingOptions>();
            options.Listen(bindingOptions.BindAddress, bindingOptions.ApplicationPort);
        });

        var startup = new Startup(webApp.Configuration);
        startup.ConfigureServices(webApp.Services);

        var host = webApp.Build();
        startup.Configure(host, webApp.Environment);

        return host;
    }

    private static async Task MigrateDatabaseAsync()
    {
        Logger!.LogInformation("Migrating database...");
        using var scope = Provider!.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ShareDbContext>();
        await dbContext.Database.MigrateAsync();
        await dbContext.DisposeAsync();
        Logger!.LogInformation("Database Migration complete!");
    }

    private static void EnsureValidMapperConfiguration()
    {
        var mapper = Provider!.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    public static async Task ShutdownAsync()
    {
        var lifetime = Provider!.GetRequiredService<IHostLifetime>();
        using var cts = new CancellationTokenSource(ShutdownTimeout);
        await lifetime.StopAsync(cts.Token);
        Environment.Exit(0);
    }
}

