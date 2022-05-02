using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;

public interface ICurrentUserService : IService
{
    public UserId GetOrThrow();
    public UserId? GetUserId();
}
