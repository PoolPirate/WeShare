using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetPostSnippet
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
    }

    public record Result(Status Status, PostSnippetDto? PostSnippet = null);

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;
        private readonly IMapper Mapper;

        public Handler(IShareContext dbContext, IAuthorizer authorizer, IMapper mapper)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
            Mapper = mapper;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var snippet = await DbContext.Posts
                .Where(x => x.Id == request.PostId)
                .ProjectTo<PostSnippetDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (snippet is null)
            {
                return new Result(Status.PostNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(request.PostId, PostQueryOperation.ReadContent, cancellationToken);
            return new Result(Status.Success, snippet);
        }
    }
}

