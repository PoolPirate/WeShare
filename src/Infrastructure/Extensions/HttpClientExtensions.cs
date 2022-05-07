namespace WeShare.Infrastructure.Extensions;
public static partial class Extensions
{
    public static async Task<(bool ResponseReceived, HttpResponseMessage? Response)> SendSafeAsync(this HttpClient client, HttpRequestMessage message, CancellationToken cancellationToken)
    {
        try
        {
            var response = await client.SendAsync(message, cancellationToken);
            return (true, response);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return (false, null);
        }
    }
}

