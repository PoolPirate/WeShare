using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetUserSnippet
{
    public class Query : IRequest<Result>
    {
        public UserId UserId { get; }

        public Query(UserId userId)
        {
            UserId = userId;
        }
    }

    public enum Status
    {
        Success,
        UserNotFound,
    }

    public record Result(Status Status, UserSnippetDto? UserSnippet);

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IMapper Mapper;

        public Handler(IShareContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var userSnippet = await DbContext.Users
                .Where(x => x.Id == request.UserId)
                .ProjectTo<UserSnippetDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return userSnippet is null
                ? new Result(Status.UserNotFound, null)
                : new Result(Status.Success, userSnippet);
        }
    }
}
