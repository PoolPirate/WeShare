using Common.Services;
using WeShare.Application.Entities;
using WeShare.Application.Services;

namespace WeShare.Infrastructure.Services;
public class PostProcessor : Singleton, IPostProcessor
{
    public ValueTask<(IDictionary<string, string[]>, Stream)> PreProcessAsync(
        IDictionary<string, string[]> headers, Stream payload, PostProcessingContext context)
        => ValueTask.FromResult((headers, payload));
}

