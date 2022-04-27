using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;

public interface IJwtService : IService
{
    public string GenerateUserLoginJWT(UserId userId, out int expiresInSeconds);
}
