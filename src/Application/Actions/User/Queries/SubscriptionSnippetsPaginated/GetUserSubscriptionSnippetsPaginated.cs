using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common.Mappings;
using WeShare.Application.Common.Models;
using WeShare.Application.Common.Security;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetUserSubscriptionSnippetsPaginated
{
    public class Query : IRequest<Result>
    {
        public UserId UserId { get; }

        [EnumDataType(typeof(SubscriptionType))]
        public SubscriptionType? Type { get; }

        [Range(0, UInt16.MaxValue)]
        public ushort Page { get; }

        [Range(3, 100)]
        public ushort PageSize { get; }

        public Query(UserId userId, SubscriptionType? type, ushort page, ushort pageSize)
        {
            UserId = userId;
            Type = type;
            Page = page;
            PageSize = pageSize;
        }
    }

    public enum Status : byte
    {
        Success,
        UserNotFound,
    }

    public record Result(Status Status, PaginatedList<SubscriptionSnippetDto>? SubscriptionInfos = null);

    [Authorize]
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
            await Authorizer.EnsureAuthorizationAsync(request.UserId, UserQueryOperation.ReadSubscriptions, cancellationToken);

            if (!await DbContext.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken))
            {
                return new Result(Status.UserNotFound);
            }

            var authenticatedUserId = CurrentUserService.GetOrThrow();

            var subscriptionInfos = await DbContext.Subscriptions
                .Where(x => x.UserId == request.UserId)
                .Where(x => !x.Share!.IsPrivate || x.Share.OwnerId == authenticatedUserId)
                .Where(x => !request.Type.HasValue || x.Type == request.Type.Value)
                .ProjectTo<SubscriptionSnippetDto>(Mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Page, request.PageSize, cancellationToken);

            return new Result(Status.Success, subscriptionInfos);
        }
    }
}

