using Common.Services;
using Microsoft.Extensions.Primitives;
using WeShare.Application.Entities;
using WeShare.Application.Services;
using WeShare.Domain.Common;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Services;
public class PostProcessor : Singleton, IPostProcessor
{
    public ValueTask<PostMetadata> PreProcessAsync(PostContent content, PostProcessingContext context)
        => ValueTask.FromResult(new PostMetadata(
                GetHeaderLength(content.Headers),
                GetStreamLength(content.Payload)
            ));

    private static ByteCount GetStreamLength(Stream stream)
        => ByteCount.From(stream.Length);

    private static ByteCount GetHeaderLength(IReadOnlyDictionary<string, StringValues> headers)
    {
        long totalLength = 0;

        foreach (var pair in headers)
        {
            totalLength += pair.Key.Length;
            totalLength += pair.Value.Sum(x => x.Length);
        }

        return ByteCount.From(totalLength);
    }
}

