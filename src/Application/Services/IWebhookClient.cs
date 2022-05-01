using Common.Services;
using System.Net;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IWebhookClient : IService
{
    public Task<(bool Success, HttpStatusCode? StatusCode, int LatencyMillis)> TrySendPostAsync(Uri targetUrl, Post post, PostContent content, CancellationToken cancellationToken);
}

