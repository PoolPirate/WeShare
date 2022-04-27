using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;
using WeShare.Domain.Enums;

namespace WeShare.Application.DTOs;
public class CallbackInfoDto : IMapFrom<Callback>
{
    public long Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public CallbackType Type { get; private set; }
}

