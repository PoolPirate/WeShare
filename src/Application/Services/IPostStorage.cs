using Common.Services;
using Microsoft.Extensions.Primitives;
using WeShare.Application.Entities;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IPostStorage : IService
{
    public Task<PostMetadata> StoreAsync(PostId postId, IDictionary<string, string[]> headers, Stream payload, CancellationToken cancellationToken);
    public Task<PostContent?> LoadAsync(PostId postId, CancellationToken cancellationToken);
    public Task DeleteAsync(PostId postId);
}
