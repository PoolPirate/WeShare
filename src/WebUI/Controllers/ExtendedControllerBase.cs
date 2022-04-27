using MediatR;

using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Services;

namespace WeShare.WebAPI.Controllers;

[ApiController]
public abstract class ExtendedControllerBase : ControllerBase
{
    private ICurrentUserService CurrentUserServiceBackingField = null!;
    protected ICurrentUserService CurrentUserService => CurrentUserServiceBackingField ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

    private ISender MediatorBackingField = null!;
    protected ISender Mediator => MediatorBackingField ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
