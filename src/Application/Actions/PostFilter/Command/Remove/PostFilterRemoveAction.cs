using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class PostFilterRemoveAction
{
    public class Command : IRequest<Result>
    {
        public PostFilterId PostFilterId { get; }

        public Command(PostFilterId postFilterId)
        {
            PostFilterId = postFilterId;
        }
    }

    public enum Status : byte
    {
        Success,
        PostFilterNotFound,
    }

    public record Result(Status Status);

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
            var postFilter = await DbContext.PostFilters
                .Where(x => x.Id == request.PostFilterId)
                .SingleOrDefaultAsync(cancellationToken);

            if (postFilter is null)
            {
                return new Result(Status.PostFilterNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(postFilter, PostFilterCommandOperation.Remove, cancellationToken);

            DbContext.PostFilters.Remove(postFilter);
            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.PostFilterNotFound),
                _ => throw new UnhandledDbStatusException(saveResult)
            };
        }
    }
}

