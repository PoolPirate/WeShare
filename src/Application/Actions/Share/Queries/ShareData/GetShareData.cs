using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetShareData
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
        OwnerNotFound,
    }

    public record Result(Status Status, ShareDataDto? ShareData = null);

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
            await Authorizer.EnsureAuthorizationAsync(request.ShareId, ShareQueryOperation.ReadData, cancellationToken);

            var shareInfo = await DbContext.Shares
                .Where(x => x.Id == request.ShareId)
                .ProjectTo<ShareInfoDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (shareInfo is null)
            {
                return new Result(Status.ShareNotFound);
            }

            var userInfo = await DbContext.Users
                .Where(x => x.Id == new UserId(shareInfo.OwnerId))
                .ProjectTo<UserSnippetDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (userInfo is null)
            {
                return new Result(Status.OwnerNotFound);
            }

            var shareData = new ShareDataDto(shareInfo, userInfo);
            return new Result(Status.Success, shareData);
        }
    }
}

