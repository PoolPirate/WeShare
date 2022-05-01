using Common.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Services;
public class WebhookClient : Singleton, IWebhookClient
{
    private readonly HttpClient HttpClient;

    public WebhookClient()
    {
        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true
        };
        HttpClient = new HttpClient(handler);
    }

    public async Task<bool> TrySendPostAsync(Uri targetUrl, Post post, PostContent content, CancellationToken cancellationToken)
    {
        try
        {
            var request = GetPostRequest(targetUrl, post, content);

            var response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            Logger.LogDebug("Timeout reached when sending post via webhook");
            return false;
        }
        catch (Exception ex)
        {
            Logger.LogCritical(ex, "There was unexpected exception while sending post via webhook");
            return false;
        }
    }

    private static HttpRequestMessage GetPostRequest(Uri targetUrl, Post post, PostContent content)
    {
        var requestContent = new ByteArrayContent(content.Payload);

        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = targetUrl,
            Content = requestContent,
        };

        request.Headers.Clear();

        foreach(var header in content.Headers)
        {
            if (header.Key == "Host" || header.Key == "Content-Length")
            {
                continue;
            }
            if (header.Key == "Content-Type")
            {
                request.Content.Headers.ContentType = new MediaTypeHeaderValue(header.Value.First());
                continue;
            }

            request.Headers.Add(header.Key, header.Value);
        }

        request.Headers.Add("Host", targetUrl.Host);

        string t = 

        return request;
    }
}

