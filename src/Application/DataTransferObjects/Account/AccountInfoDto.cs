using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;

public class AccountInfoDto : IMapFrom<User>
{
    public string Email { get; private set; } = null!;
    public bool EmailVerified { get; private set; }
}
