using Common.Services;
using MediatR;

namespace WeShare.Application.Services;
public interface IDispatcher : IService
{
    public void Enqueue(IRequest request, string jobName);
}
