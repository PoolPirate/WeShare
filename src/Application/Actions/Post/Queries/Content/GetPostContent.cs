using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetPostContent
{
    public class Query : IRequest<Result>
    {
        public PostId PostId { get; }

        public Query(PostId postId)
        {
            PostId = postId;
        }
    }

    public enum Status : byte
    {
        Success,
        PostNotFound,
        ContentNotFound,
    }

    public record Result(Status Status, PostContent? PostContent = null);

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;
        private readonly IPostStorage PostStorage;

        public Handler(IShareContext dbContext, IAuthorizer authorizer, IPostStorage postStorage)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
            PostStorage = postStorage;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            if (!await DbContext.Posts.AnyAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken))
            {
                return new Result(Status.PostNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(request.PostId, PostQueryOperation.ReadContent, cancellationToken);

            var content = await PostStorage.LoadAsync(request.PostId, cancellationToken);

            return content is null
                ? new Result(Status.ContentNotFound)
                : new Result(Status.Success, content);
        }
    }
}

