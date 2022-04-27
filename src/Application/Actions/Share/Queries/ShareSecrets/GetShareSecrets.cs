using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetShareSecrets
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
        ShareNotFound
    }

    public record Result(Status Status, ShareSecretsDto? ShareSecrets = null);

    [Authorize]
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
            await Authorizer.EnsureAuthorizationAsync(request.ShareId, ShareQueryOperation.ReadSecrets, cancellationToken);

            var shareSecrets = await DbContext.Shares
                .Where(x => x.Id == request.ShareId)
                .ProjectTo<ShareSecretsDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return shareSecrets is null
                ? new Result(Status.ShareNotFound)
                : new Result(Status.Success, shareSecrets);
        }
    }
}

