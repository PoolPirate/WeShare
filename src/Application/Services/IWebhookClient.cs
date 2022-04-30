using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IWebhookClient : IService
{
    public Task<bool> TrySendPostAsync(Uri targetUrl, Post post, PostContent content, CancellationToken cancellationToken);
}

