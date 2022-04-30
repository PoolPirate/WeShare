using Common.Services;
using Microsoft.Extensions.Primitives;
using WeShare.Application.Entities;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IPostProcessor : IService
{
    public ValueTask<(IDictionary<string, string[]>, Stream)> PreProcessAsync(IDictionary<string, string[]> headers, Stream payload, PostProcessingContext context);
}
