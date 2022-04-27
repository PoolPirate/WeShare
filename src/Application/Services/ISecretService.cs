using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;

public interface ISecretService : IService
{
    string HashPassword(PlainTextPassword plainPassword);
    bool VerifyPassword(string passwordHash, PlainTextPassword input);

    CallbackSecret GenerateCallbackSecret();
    ShareSecret GenerateShareSecret();
}
