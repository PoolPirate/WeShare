using Common.Services;
using WeShare.Application.Entities;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Services;
public class PostProcessor : Singleton, IPostProcessor
{
    public async ValueTask<PostContent> PreProcessAsync(
        IDictionary<string, string[]> headers, Stream payload, PostProcessingContext context)
    {
        var memStream = new MemoryStream();
        await payload.CopyToAsync(memStream);

        return new PostContent(headers, memStream.ToArray());
    }
}

