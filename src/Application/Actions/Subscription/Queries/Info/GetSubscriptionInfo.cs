using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetSubscriptionInfo
{
    public class Query : IRequest<Result>
    {
        public SubscriptionId SubscriptionId { get; }

        public Query(SubscriptionId subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }
    }

    public enum Status : byte
    {
        Success,
        SubscriptionNotFound,
    }

    public record Result(Status Status, SubscriptionInfoDto? SubscriptionInfo = null);

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
            await Authorizer.EnsureAuthorizationAsync(request.SubscriptionId, SubscriptionQueryOperation.ReadInfo, cancellationToken);

            var subscriptionInfo = await DbContext.Subscriptions
                .Where(x => x.Id == request.SubscriptionId)
                .ProjectTo<SubscriptionInfoDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return subscriptionInfo is null
                ? new Result(Status.SubscriptionNotFound)
                : new Result(Status.Success, subscriptionInfo);
        }
    }
}

