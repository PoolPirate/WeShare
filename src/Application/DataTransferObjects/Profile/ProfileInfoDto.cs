using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;

public class ProfileInfoDto : IMapFrom<User>
{
    public string? Nickname { get; private set; }
    public bool LikesPublished { get; private set; }
}
