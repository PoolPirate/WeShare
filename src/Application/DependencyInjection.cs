using Common.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WeShare.Application.Common.Behaviours;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using WeShare.Application.Actions.Tasks;

namespace WeShare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

        services.AddScoped(provider => provider.GetRequiredService<IInjector>().CreateServiceInstance<PostPublisher<WebhookSubscription>>(provider));
        services.AddScoped(provider => provider.GetRequiredService<IInjector>().CreateServiceInstance<PostPublisher<DiscordSubscription>>(provider));

        services.AddScoped<IRequestHandler<PostPublishTask.Command<WebhookSubscription>, Unit>, PostPublishTask.Handler<WebhookSubscription>>();
        services.AddScoped<IRequestHandler<PostPublishTask.Command<DiscordSubscription>, Unit>, PostPublishTask.Handler<DiscordSubscription>>();

        return services;
    }
}
