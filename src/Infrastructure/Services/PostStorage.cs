using Common.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Text.Json;
using WeShare.Application.Entities;
using WeShare.Application.Services;
using WeShare.Domain.Common;
using WeShare.Domain.Entities;
using WeShare.Infrastructure.Options;

namespace WeShare.Infrastructure.Services;
public class PostStorage : Singleton, IPostStorage
{
    [Inject]
    private readonly StorageOptions StorageOptions;

    protected override ValueTask InitializeAsync()
    {
        if (!Directory.Exists(StorageOptions.PostStoragePath))
        {
            Directory.CreateDirectory(StorageOptions.PostStoragePath);
        }

        return ValueTask.CompletedTask;
    }

    private string GetPostStoragePath(string filename)
        => Path.Combine(StorageOptions.PostStoragePath, filename);

    private static string GetHeaderFilename(PostId postId)
        => $"{postId}.header";
    private static string GetPayloadFilename(PostId postId)
        => $"{postId}.payload";

    public async Task<PostContent?> LoadAsync(PostId postId, CancellationToken cancellationToken)
    {
        var headersTask = FindPostHeadersAsync(postId, cancellationToken);
        var payloadTask = FindPostPayloadAsync(postId, cancellationToken);

        var headers = await headersTask;
        byte[]? payload = await payloadTask;

        if (headers is null || payload is null)
        {
            return null;
        }

        var content = new PostContent(headers, payload);
        return content;
    }

    private async Task<IDictionary<string, string[]>?> FindPostHeadersAsync(PostId postId, CancellationToken cancellationToken)
    {
        string path = GetPostStoragePath(GetHeaderFilename(postId));
        if (!File.Exists(path))
        {
            return null;
        }

        string headersJson = await File.ReadAllTextAsync(path, cancellationToken);

        return String.IsNullOrWhiteSpace(headersJson)
            ? null
            : JsonSerializer.Deserialize<Dictionary<string, string[]>>(headersJson);
    }

    private async Task<byte[]?> FindPostPayloadAsync(PostId postId, CancellationToken cancellationToken)
    {
        string path = GetPostStoragePath(GetPayloadFilename(postId));

        if (!File.Exists(path))
        {
            return null;
        }
        //
        return await File.ReadAllBytesAsync(path, cancellationToken);
    }

    public async Task<PostMetadata> StoreAsync(PostId postId, IDictionary<string, string[]> headers, Stream payload, CancellationToken cancellationToken)
{
        var storeHeadersTask = StorePostHeadersAsync(postId, headers);
        var storeBodyTask = StorePostPayloadAsync(postId, payload);

        var headersSize = await storeHeadersTask;
        var bodySize = await storeBodyTask;

        return new PostMetadata(headersSize, bodySize);
    }

    private async Task<ByteCount> StorePostHeadersAsync(PostId postId, IDictionary<string, string[]> headers)
    {
        string path = GetPostStoragePath(GetHeaderFilename(postId));

        if (File.Exists(path))
        {
            throw new InvalidOperationException("Trying to store post headers that have already been stored!");
        }

        string headersJson = JsonSerializer.Serialize(headers);
        byte[] headersBytes = Encoding.UTF8.GetBytes(headersJson);
        await File.WriteAllBytesAsync(path, headersBytes);

        Logger.LogInformation("Headers save success: ; PostId={postId} ; Size={size}", postId, headersBytes.Length);
        return ByteCount.From(headersBytes.Length);
    }

    private async Task<ByteCount> StorePostPayloadAsync(PostId postId, Stream payload)
    {
        string path = GetPostStoragePath(GetPayloadFilename(postId));

        if (File.Exists(path))
        {
            throw new InvalidOperationException("Trying to store post body that has already been stored!");
        }

        var fileStream = File.OpenWrite(path);
        try
        {
            await payload.CopyToAsync(fileStream);
            await fileStream.FlushAsync();

            Logger.LogInformation("Body save success: PostId={postId} ; Size={size}", postId, fileStream.Position);
            return ByteCount.From(fileStream.Position);
        }
        finally
        {
            await fileStream.DisposeAsync();
        }
    }

    public Task DeleteAsync(PostId postId)
    {
        File.Delete(GetPostStoragePath(GetHeaderFilename(postId)));
        File.Delete(GetPostStoragePath(GetPayloadFilename(postId)));
        return Task.CompletedTask;
    }
}

