using Common.Services;
using WeShare.Domain.Common;

namespace WeShare.Application.Services;

public interface IDomainEventService : IService
{
    Task Publish(DomainEvent domainEvent);
}
