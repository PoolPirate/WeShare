using Common.Services;
using WeShare.Application.Entities;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IPostProcessor : IService
{
    public ValueTask<PostMetadata> PreProcessAsync(PostContent content, PostProcessingContext context);
}
