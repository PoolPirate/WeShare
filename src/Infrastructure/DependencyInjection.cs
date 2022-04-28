using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using System.Security.Cryptography;
using WeShare.Application.Services;
using WeShare.Infrastructure.Options;
using WeShare.Infrastructure.Persistence;
using WeShare.Infrastructure.Persistence.Concurrency;

namespace WeShare.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {

        services.AddDbContext<IShareContext, ShareDbContext>((provider, options) =>
        {
            var dbOptions = provider.GetRequiredService<DatabaseOptions>();
            options.UseNpgsql(dbOptions.AppConnectionString,
                b => b.MigrationsAssembly(typeof(ShareDbContext).Assembly.FullName));
        });

        services.AddHangfire((provider, options)
         => options
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(provider.GetRequiredService<DatabaseOptions>().HangfireConnectionString, new PostgreSqlStorageOptions()
            {
                PrepareSchemaIfNecessary = true,
                EnableTransactionScopeEnlistment = true,
            }));

        services.AddHangfireServer();

        services.AddSendGrid((provider, options) =>
        {
            var sendGridOptions = provider.GetRequiredService<SendGridOptions>();
            options.ApiKey = sendGridOptions.ApiKey;
        });

        services.AddSingleton<Random>();
        services.AddSingleton<PropertyMerger>();
        services.AddSingleton(RandomNumberGenerator.Create());

        return services;
    }
}
