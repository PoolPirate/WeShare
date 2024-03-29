﻿using AutoMapper;
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
public class GetSubscriptionReceivedPostSnippetsPaginated
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
        SubscriptionNotFound
    }

    public record Result(Status Status, PaginatedList<PostSendInfoDto>? PostSnippets = null);

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
            await Authorizer.EnsureAuthorizationAsync(request.SubscriptionId, SubscriptionQueryOperation.ReadReceivedPosts, cancellationToken);

            var postSendInfos = await DbContext.SentPosts
                .Where(x => x.SubscriptionId == request.SubscriptionId)
                .Where(x => x.Received)
                .OrderByDescending(x => x.PostId)
                .ProjectTo<PostSendInfoDto>(Mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Page, request.PageSize, cancellationToken);

            if (postSendInfos.TotalCount == 0)
            {
                if (!await DbContext.Subscriptions.AnyAsync(x => x.Id == request.SubscriptionId, cancellationToken))
                {
                    return new Result(Status.SubscriptionNotFound);
                }
            }

            var postIds = postSendInfos.Items.Select(x => new PostId(x.PostSnippet.Id)).ToArray();

            var postSendfailures = await DbContext.PostSendFailures
                .Where(x => postIds.Contains(x.PostId))
                .Where(x => x.SubscriptionId == request.SubscriptionId)
                .ToListAsync(cancellationToken);

            var postSendfailureDtos = postSendfailures.GroupBy(x => x.PostId)
                .Select(x => new { x.Key, DTOs = x.Select(x => (object)Mapper.Map<PostSendFailureDto>(x)).ToList() })
                .ToList();

            foreach (var item in postSendInfos.Items)
            {
                item.PostSendFailures = postSendfailureDtos.SingleOrDefault(x => x.Key.Value == item.PostSnippet.Id)?.DTOs ?? new List<object>();
            }

            return new Result(Status.Success, postSendInfos);
        }
    }
}

