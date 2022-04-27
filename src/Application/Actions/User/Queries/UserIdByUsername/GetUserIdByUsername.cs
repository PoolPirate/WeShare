using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetUserIdByUsername
{
    public class Query : IRequest<Result>
    {
        public Username Username { get; }

        public Query(Username username)
        {
            Username = username;
        }
    }

    public enum Status : byte
    {
        Success,
        UserNotFound,
    }

    public record Result(Status Status, UserId? UserId = null);

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IShareContext DbContext;

        public Handler(IShareContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var userIdData = await DbContext.Users
            .Where(x => x.Username == request.Username)
            .Select(x => new { UserId = x.Id })
            .SingleOrDefaultAsync(cancellationToken);

            return userIdData is null
                ? new Result(Status.UserNotFound)
                : new Result(Status.Success, userIdData.UserId);
        }
    }
}

