using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IWebhookClient : IService
{
    public Task<bool> TrySendPostAsync(Post post, PostContent content, Subscription subscription, CancellationToken cancellationToken);
}

