using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class DiscordConnectionCreateAction
{
    public class Command : IRequest<Result>
    {
        public UserId UserId { get; }

        [Required]
        [MinLength(6)]
        public string Code { get; }

        public Command(UserId userId, string code)
        {
            UserId = userId;
            Code = code;
        }
    }

    public enum Status : byte
    {
        Success,
        UserNotFound,
        InvalidCode,
        MissingScope,
        DiscordUserIdCouldNotBeLoaded,
        FailedAddingToGuild,
        TargetUserAlreadyLinked,
    }

    public record Result(Status Status);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly ILogger Logger;
        private readonly IAuthorizer Authorizer;
        private readonly IOAuth2ExchangeService OAuth2ExchangeService;
        private readonly IDiscordClient DiscordClient;

        public Handler(IShareContext dbContext, ILogger<DiscordConnectionCreateAction> logger, IAuthorizer authorizer, IOAuth2ExchangeService oAuth2ExchangeService, IDiscordClient discordClient)
        {
            DbContext = dbContext;
            Logger = logger;
            Authorizer = authorizer;
            OAuth2ExchangeService = oAuth2ExchangeService;
            DiscordClient = discordClient;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await DbContext.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken))
            {
                return new Result(Status.UserNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(request.UserId, ServiceConnectionCommandOperation.Create, cancellationToken);

            var tokenResponse = await OAuth2ExchangeService.ExchangeCodeForTokens(ServiceConnectionType.Discord, request.Code, cancellationToken);

            if (tokenResponse is null)
            {
                Logger.LogWarning("Failed creating DiscordConnection, invalid code");
                return new Result(Status.InvalidCode);
            }

            if (!ValidateScopes(tokenResponse.Scope))
            {
                Logger.LogWarning("Failed creating DiscordConnection, missing scope");
                return new Result(Status.MissingScope);
            }

            var discordUserId = await DiscordClient.LoadDiscordUserIdAsync(tokenResponse.AccessToken, cancellationToken);

            if (!discordUserId.HasValue)
            {
                Logger.LogWarning("Failed creating DiscordConnection, DiscordUserId could not be loaded");
                return new Result(Status.DiscordUserIdCouldNotBeLoaded);
            }

            if (!await DiscordClient.AddUserToWeShareGuild(tokenResponse.AccessToken, discordUserId.Value, cancellationToken))
            {
                Logger.LogWarning("Failed creating DiscordConnection, failed adding to guild");
                return new Result(Status.FailedAddingToGuild);
            }

            var discordConnection = DiscordConnection.Create(request.UserId, discordUserId.Value);
            DbContext.DiscordConnections.Add(discordConnection);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.DuplicateIndex, cancellationToken: cancellationToken);

            _ = DiscordClient.SendWelcomeMessage(discordUserId.Value);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.DuplicateIndex => new Result(
                    saveResult.IsConflictingIndex(typeof(ServiceConnection), nameof(DiscordConnection.DiscordId))
                        ? Status.TargetUserAlreadyLinked
                        : throw new UnhandledIndexConflictException(saveResult)
                ),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }

        private static readonly string[] RequiredScopes = new[]
        {
            "identify",
            "guilds.join"
        };

        private static bool ValidateScopes(string scope)
        {
            foreach (string requiredScope in RequiredScopes)
            {
                if (!scope.Contains(requiredScope))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

