using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class ServiceConnectionCreateAction
{
    public class Command : IRequest<Result>
    {
        public UserId UserId { get; }

        [EnumDataType(typeof(ServiceConnectionType))]
        public ServiceConnectionType Type { get; }

        [Required]
        [MinLength(6)]
        public string Code { get; }

        public Command(UserId userId, ServiceConnectionType type, string code)
        {
            UserId = userId;
            Type = type;
            Code = code;
        }
    }

    public enum Status : byte
    {
        Success,
        UserNotFound,
        InvalidCode,
        FailedRetrievingUserId,
        TargetAlreadyLinked,
    }

    public record Result(Status Status);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;
        private readonly IOAuth2ExchangeService OAuth2ExchangeService;

        private readonly IDiscordClient DiscordClient;

        public Handler(IShareContext dbContext, IAuthorizer authorizer, IOAuth2ExchangeService oAuth2ExchangeService, 
            IDiscordClient discordClient)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
            OAuth2ExchangeService = oAuth2ExchangeService;
            DiscordClient = discordClient;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await Authorizer.EnsureAuthorizationAsync(request.UserId, ServiceConnectionCommandOperation.Create, cancellationToken);

            if (!await DbContext.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken))
            {
                return new Result(Status.UserNotFound);
            }

            string? accessToken = await OAuth2ExchangeService.RetrieveAccessTokenAsync(request.Type, request.Code, cancellationToken);

            if (accessToken is null)
            {
                return new Result(Status.InvalidCode);
            }

            bool success = request.Type switch
            {
                ServiceConnectionType.Discord => await HandleDiscordAccessTokenAsync(request.UserId, accessToken, cancellationToken),

                ServiceConnectionType.None or _
                    => throw new InvalidOperationException(),
            };

            if (!success)
            {
                return new Result(Status.FailedRetrievingUserId);
            }

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.DuplicateIndex, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.DuplicateIndex => new Result(
                    saveResult.IsConflictingIndex(typeof(ServiceConnection), nameof(DiscordConnection.DiscordId))
                        ? Status.TargetAlreadyLinked
                        : throw new UnhandledIndexConflictException(saveResult)
                    ),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }

        private async Task<bool> HandleDiscordAccessTokenAsync(UserId userId, string accessToken, CancellationToken cancellationToken)
        {
            var discordId = await DiscordClient.LoadDiscordIdAsync(accessToken, cancellationToken);

            if (!discordId.HasValue)
            {
                return false;
            }

            var discordConnection = DiscordConnection.Create(userId, discordId.Value);
            DbContext.DiscordConnections.Add(discordConnection);
            return true;
        }
    }
}

