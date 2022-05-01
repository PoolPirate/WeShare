using Common.Services;
using WeShare.Application.Entities;

namespace WeShare.Application.Services;
public interface IPostProcessor : IService
{
    public ValueTask<(IDictionary<string, string[]>, Stream)> PreProcessAsync(IDictionary<string, string[]> headers, Stream payload, PostProcessingContext context);
}
