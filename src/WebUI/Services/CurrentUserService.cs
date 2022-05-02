using Common.Services;
using System.Security.Claims;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Services;

public class CurrentUserService : Scoped, ICurrentUserService
{
    [Inject]
    private readonly IHttpContextAccessor HttpContextAccessor;

    public UserId GetOrThrow()
    {
        string? idStr = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return idStr is null 
            ? throw new UnauthorizedAccessException() 
            : new UserId(Int64.Parse(idStr));
    }

    public UserId? GetUserId()
    {
        string? idStr = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return idStr is null
            ? null
            : new UserId(Int64.Parse(idStr));
    }
}
