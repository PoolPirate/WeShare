using System.Net;
using System.Net.Http.Json;

namespace WeShare.Application.Services;

public class DiscordResponse
{
    public DiscordStatus Status { get; }

    public DiscordResponse(DiscordStatus status)
    {
        Status = status;
    }

    public static DiscordResponse FromTimeout()
        => new DiscordResponse(DiscordStatus.Unavailable);

    public static DiscordResponse FromHttpResponse(HttpResponseMessage response)
    {
        var status = response.StatusCode switch
        {
            HttpStatusCode.OK or HttpStatusCode.Created or HttpStatusCode.NoContent
                => DiscordStatus.Success,
            HttpStatusCode.Forbidden
                => DiscordStatus.Forbidden,
            HttpStatusCode.TooManyRequests
                => DiscordStatus.RateLimited,
            _ => throw new HttpRequestException($"Unhandled discord HTTP status code: {response.StatusCode}")
        };

        return new DiscordResponse(status);
    }
}

public class DiscordResponse<T> : DiscordResponse
{
    public T? Value { get; }

    public DiscordResponse(DiscordStatus status, T? value)
        : base(status)
    {
        Value = value;
    }

    public static new DiscordResponse<T> FromTimeout()
        => new DiscordResponse<T>(DiscordStatus.Unavailable, default);

    public static async Task<DiscordResponse<T>> FromHttpResponseAsync(HttpResponseMessage response)
    {
        var status = response.StatusCode switch
        {
            HttpStatusCode.OK or HttpStatusCode.Created or HttpStatusCode.NoContent
                => DiscordStatus.Success,
            HttpStatusCode.Forbidden
                => DiscordStatus.Forbidden,
            HttpStatusCode.TooManyRequests
                => DiscordStatus.RateLimited,
            _ => throw new HttpRequestException($"Unhandled discord HTTP status code: {response.StatusCode}")
        };

        var value = await (status == DiscordStatus.Success
            ? response.Content.ReadFromJsonAsync<T>()
            : Task.FromResult<T?>(default));

        return new DiscordResponse<T>(status, value);
    }

    public static async Task<DiscordResponse<T>> FromHttpResponseAsync<TSource>(HttpResponseMessage response, Func<TSource, T> mapping)
    {
        var status = response.StatusCode switch
        {
            HttpStatusCode.OK or HttpStatusCode.Created or HttpStatusCode.NoContent
                => DiscordStatus.Success,
            HttpStatusCode.Forbidden
                => DiscordStatus.Forbidden,
            HttpStatusCode.TooManyRequests
                => DiscordStatus.RateLimited,
            _ => throw new HttpRequestException($"Unhandled discord HTTP status code: {response.StatusCode}")
        };

        if (status != DiscordStatus.Success)
        {
            return new DiscordResponse<T>(status, default);
        }

        var sourceValue = await response.Content.ReadFromJsonAsync<TSource>();

        if (sourceValue is null)
        {
            return new DiscordResponse<T>(status, default);
        }

        var value = mapping.Invoke(sourceValue);
        return new DiscordResponse<T>(status, value);
    }
}

