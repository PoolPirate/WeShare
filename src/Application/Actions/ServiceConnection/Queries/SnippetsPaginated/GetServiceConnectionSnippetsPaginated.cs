using AutoMapper;
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
public class GetServiceConnectionSnippetsPaginated
{
    public class Query : IRequest<Result>
    {
        public UserId UserId { get; }

        [Range(0, UInt16.MaxValue)]
        public ushort Page { get; }

        [Range(3, 100)]
        public ushort PageSize { get; }

        public Query(UserId userId, ushort page, ushort pageSize)
        {
            UserId = userId;
            Page = page;
            PageSize = pageSize;
        }
    }

    public enum Status : byte
    {
        Success,
        UserNotFound,
    }

    public record Result(Status Status, PaginatedList<object>? ServiceConnectionSnippets = null);

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
            await Authorizer.EnsureAuthorizationAsync(request.UserId, ServiceConnectionQueryOperation.ReadSnippets, cancellationToken);

            var connections = await DbContext.ServiceConnections
                .Where(x => x.UserId == request.UserId)
                .PaginatedListAsync(request.Page, request.PageSize, cancellationToken);

            if (connections.TotalCount == 0)
            {
                if (!await DbContext.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken))
                {
                    return new Result(Status.UserNotFound);
                }
            }

            var snippets = new List<object>();

            foreach(var connection in connections.Items)
            {
                snippets.Add(Mapper.Map<ServiceConnectionSnippetDto>(connection));
            }

            var connectionSnippets = new PaginatedList<object>(
                snippets, connections.TotalCount, connections.PageNumber, request.PageSize);

            return new Result(Status.Success, connectionSnippets);
        }
    }
}

