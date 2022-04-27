using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;

/// <summary>
/// A object representing all public user information.
/// </summary>
public class UserSnippetDto : IMapFrom<User>
{
    //Account Parts
    public long Id { get; private set; }
    public string Username { get; private set; } = null!;

    //Profile Parts
    public DateTimeOffset CreatedAt { get; private set; }
    public string? Nickname { get; private set; }
}
