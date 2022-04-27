using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetAccountInfo
{
    public class Query : IRequest<Result>
    {
        public UserId UserId { get; }

        public Query(UserId userId)
        {
            UserId = userId;
        }
    }

    public enum Status : byte
    {
        Success,
        UserNotFound,
    }

    public record Result(Status Status, AccountInfoDto? AccountInfo = null);

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
            await Authorizer.EnsureAuthorizationAsync(request.UserId, UserQueryOperation.ReadAccount, cancellationToken);

            var accountInfo = await DbContext.Users
                .Where(x => x.Id == request.UserId)
                .ProjectTo<AccountInfoDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return accountInfo is null
                ? new Result(Status.UserNotFound)
                : new Result(Status.Success, accountInfo);
        }
    }
}

