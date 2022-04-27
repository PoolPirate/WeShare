using Common.Services;
using WeShare.Domain.Entities;
using WeShare.Domain.Enums;

namespace WeShare.Application.Services;
public interface ICallbackService : IService
{
    Callback Create(UserId userId, CallbackType type);
    bool Validate(Callback? callback);
}
