using Common.Services;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Services;
public class PostStorage : Singleton, IPostStorage
{
    public Task<PostContent> LoadAsync(PostId postId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task StoreAsync(PostId postId, PostContent content, CancellationToken cancellationToken) => throw new NotImplementedException();
}

