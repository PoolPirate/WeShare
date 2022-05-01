using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common.Mappings;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetSubscriptionUnsentPostSnippetsPaginated
{
    public class Query : IRequest<Result>
    {
        public SubscriptionId SubscriptionId { get; }

        [Range(0, UInt16.MaxValue)]
        public ushort Page { get; }

        [Range(3, 100)]
        public ushort PageSize { get; }

        public Query(SubscriptionId subscriptionId, ushort page, ushort pageSize)
        {
            SubscriptionId = subscriptionId;
            Page = page;
            PageSize = pageSize;
        }
    }

    public enum Status : byte
    {
        Success,
        SubscriptionNotFound,
    }

    public record Result(Status Status, PaginatedList<PostSnippetDto>? PostSnippets = null);

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
            await Authorizer.EnsureAuthorizationAsync(request.SubscriptionId, SubscriptionQueryOperation.ReadUnreceivedPosts, cancellationToken);

            var subscriptionData = await DbContext.Subscriptions
                .Where(x => x.Id == request.SubscriptionId)
                .Select(x => new { x.ShareId, x.CreatedAt })
                .SingleOrDefaultAsync(cancellationToken);

            if (subscriptionData is null)
            {
                return new Result(Status.SubscriptionNotFound);
            }

            var postSnippets = await DbContext.Posts
                .Where(x => x.ShareId == subscriptionData.ShareId)
                .Where(x => x.CreatedAt >= subscriptionData.CreatedAt)
                .Where(x => !x.SentPosts!
                    .Any(x => x.SubscriptionId == request.SubscriptionId))
                .ProjectTo<PostSnippetDto>(Mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Page, request.PageSize, cancellationToken);

            return new Result(Status.Success, postSnippets);
        }
    }
}

