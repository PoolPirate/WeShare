using Common.Services;
using Microsoft.Extensions.Primitives;
using WeShare.Application.Entities;
using WeShare.Application.Services;
using WeShare.Domain.Common;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Services;
public class PostProcessor : Singleton, IPostProcessor
{
    public ValueTask<(IDictionary<string, string[]>, Stream)> PreProcessAsync(
        IDictionary<string, string[]> headers, Stream payload, PostProcessingContext context)
        => ValueTask.FromResult((headers, payload));
}

