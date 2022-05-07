using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetCallbackInfo
{
    public class Query : IRequest<Result>
    {
        public CallbackSecret CallbackSecret { get; }

        public Query(CallbackSecret callbackSecret)
        {
            CallbackSecret = callbackSecret;
        }
    }

    public enum Status : byte
    {
        Success,
        CallbackNotFound,
    }

    public record Result(Status Status, CallbackInfoDto? CallbackInfo = null);

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
            var callbackInfo = await DbContext.Callbacks
                .Where(x => x.Secret == request.CallbackSecret)
                .ProjectTo<CallbackInfoDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return callbackInfo is null
                ? new Result(Status.CallbackNotFound)
                : new Result(Status.Success, callbackInfo);
        }
    }
}

