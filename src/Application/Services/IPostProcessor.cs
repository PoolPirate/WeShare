using Common.Services;
using WeShare.Application.Entities;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IPostProcessor : IService
{
    public ValueTask<PostContent> PreProcessAsync(IDictionary<string, string[]> headers, Stream payload, PostProcessingContext context);
}
