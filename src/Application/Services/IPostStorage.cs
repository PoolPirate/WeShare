using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IPostStorage : IService
{
    public Task StoreAsync(PostId postId, PostContent content, CancellationToken cancellationToken);
    public Task<PostContent> LoadAsync(PostId postId, CancellationToken cancellationToken);
}
