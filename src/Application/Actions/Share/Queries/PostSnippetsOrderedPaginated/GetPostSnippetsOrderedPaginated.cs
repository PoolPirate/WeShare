﻿using AutoMapper;
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
public class GetPostSnippetsOrderedPaginated
{
    public class Query : IRequest<Result>
    {
        public ShareId ShareId { get; }

        [EnumDataType(typeof(PostOrdering))]
        public PostOrdering Ordering { get; }

        [Range(0, UInt16.MaxValue)]
        public ushort Page { get; }

        [Range(3, 100)]
        public ushort PageSize { get; }

        public Query(ShareId shareId, PostOrdering ordering, ushort page, ushort pageSize)
        {
            ShareId = shareId;
            Ordering = ordering;
            Page = page;
            PageSize = pageSize;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNotFound,
    }

    public record Result(Status Status, PaginatedList<PostSnippetDto>? Metadatas = null);

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
            await Authorizer.EnsureAuthorizationAsync(request.ShareId, ShareQueryOperation.ReadPosts, cancellationToken);

            var metaDatas = await DbContext.Posts
                .Where(x => x.ShareId == request.ShareId)
                .OrderBy(request.Ordering)
                .ProjectTo<PostSnippetDto>(Mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Page, request.PageSize, cancellationToken);

            return metaDatas.TotalCount == 0 && !await DbContext.Shares.AnyAsync(x => x.Id == request.ShareId, cancellationToken)
                ? new Result(Status.ShareNotFound)
                : new Result(Status.Success, metaDatas);
        }
    }
}

