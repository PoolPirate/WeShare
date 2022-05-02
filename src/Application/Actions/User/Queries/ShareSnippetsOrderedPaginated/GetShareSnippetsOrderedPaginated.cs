using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common.Extensions;
using WeShare.Application.Common.Mappings;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetShareSnippetsOrderedPaginated
{
    public class Query : IRequest<Result>
    {
        public UserId UserId { get; }

        [EnumDataType(typeof(ShareOrdering))]
        public ShareOrdering ShareOrdering { get; }

        [Range(0, UInt16.MaxValue)]
        public ushort Page { get; }

        [Range(3, 100)]
        public ushort PageSize { get; }

        public Query(UserId userId, ShareOrdering shareOrdering, ushort page, ushort pageSize)
        {
            UserId = userId;
            ShareOrdering = shareOrdering;
            Page = page;
            PageSize = pageSize;
        }
    }

    public enum Status : byte
    {
        Success,
        UserNotFound,
    }

    public record Result(Status Status, PaginatedList<ShareSnippetDto>? PopularShareSnippets = null);

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;
        private readonly IMapper Mapper;
        private readonly ICurrentUserService CurrentUserService;

        public Handler(IShareContext dbContext, IAuthorizer authorizer, IMapper mapper, ICurrentUserService currentUserService)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
            Mapper = mapper;
            CurrentUserService = currentUserService;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            await Authorizer.EnsureAuthorizationAsync(request.UserId, UserQueryOperation.ReadPublicShareSnippets, cancellationToken);

            var authenticatedUserId = CurrentUserService.GetOrThrow();

            var shareSnippets = await DbContext.Shares
                .Where(x => x.OwnerId == request.UserId)
                .Where(x => !x.IsPrivate || x.OwnerId == authenticatedUserId)
                .OrderByDescending(request.ShareOrdering)
                .ProjectTo<ShareSnippetDto>(Mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Page, request.PageSize, cancellationToken);

            if (shareSnippets.TotalCount == 0 &&
                !await DbContext.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken))
            {
                return new Result(Status.UserNotFound);
            }
            //
            return new Result(Status.Success, shareSnippets);
        }
    }
}

