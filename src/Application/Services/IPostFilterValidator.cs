using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IPostFilterValidator : IService
{
    public Task<bool> ValidateFilterAsync(PostFilter filter, PostContent content);
}
