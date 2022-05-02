using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;

public class ShareDeleteAction
{
    public class Command : IRequest<Result>
    {
        public ShareId ShareId { get; }

        public Command(ShareId shareId)
        {
            ShareId = shareId;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNotFound,
    }

    public record Result(Status Status);

    [Authorize]
    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;

        public Handler(IShareContext dbContext, IAuthorizer authorizer)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var share = await DbContext.Shares
                .Where(x => x.Id == request.ShareId)
                .SingleOrDefaultAsync(cancellationToken);

            if (share is null)
            {
                return new Result(Status.ShareNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(share, ShareCommandOperation.Delete, cancellationToken);

            DbContext.Shares.Remove(share);
            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, cancellationToken: cancellationToken);
            return new Result(Status.Success);
        }
    }
}

