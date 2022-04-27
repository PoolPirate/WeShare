using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetProfileInfo
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

    public record Result(Status Status, ProfileInfoDto? ProfileInfo = null);

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
            await Authorizer.EnsureAuthorizationAsync(request.UserId, UserQueryOperation.ReadProfile, cancellationToken);

            var profileInfo = await DbContext.Users
                .Where(x => x.Id == request.UserId)
                .ProjectTo<ProfileInfoDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return profileInfo is null
                ? new Result(Status.UserNotFound)
                : new Result(Status.Success, profileInfo);
        }
    }
}

