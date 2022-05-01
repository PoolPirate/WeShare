using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetShareSnippet
{
    public class Query : IRequest<Result>
    {
        public ShareId ShareId { get; }

        public Query(ShareId shareId)
        {
            ShareId = shareId;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNotFound,
    }

    public record Result(Status Status, ShareSnippetDto? ShareSnippet = null);

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
            var shareSnippet = await DbContext.Shares
                .Where(x => x.Id == request.ShareId)
                .ProjectTo<ShareSnippetDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (shareSnippet is null)
            {
                return new Result(Status.ShareNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(request.ShareId, ShareQueryOperation.ReadSnippet, cancellationToken);
            return new Result(Status.Success, shareSnippet);
        }
    }
}

