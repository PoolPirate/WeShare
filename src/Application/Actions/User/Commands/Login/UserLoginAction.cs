using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class UserLoginAction
{
    public class Command : IRequest<Result>
    {
        [MinLength(DomainConstraints.UsernameLengthMinimum)]
        [MaxLength(DomainConstraints.UsernameLengthMaximum)]
        [Required]
        public Username Username { get; }

        public PlainTextPassword Password { get; }

        public Command(Username username, PlainTextPassword password)
        {
            Username = username;
            Password = password;
        }
    }

    public enum Status
    {
        Success,
        UserNotFound,
        WrongPassword,
    }

    public record Result(Status Status, UserLoginDto? UserLogin);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly ILogger Logger;

        private readonly IShareContext DbContext;
        private readonly ISecretService SecretService;
        private readonly IJwtService JwtService;
        private readonly IMapper Mapper;

        public Handler(ILogger<UserLoginAction> logger, IShareContext dbContext,
            ISecretService secretService, IJwtService jwtService, IMapper mapper)
        {
            Logger = logger;
            DbContext = dbContext;
            SecretService = secretService;
            JwtService = jwtService;
            Mapper = mapper;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var loginData = await DbContext.Users
                .Where(x => x.Username == request.Username)
                .Select(x => new { x.Id, x.PasswordHash })
                .SingleOrDefaultAsync(cancellationToken);

            if (loginData is null)
            {
                Logger.LogDebug("Account Login failed: NotFound [Username={@Username}]", request.Username);
                return new Result(Status.UserNotFound, null);
            }
            if (!SecretService.VerifyPassword(loginData.PasswordHash, request.Password))
            {
                Logger.LogDebug("Login failed: PasswordMismatch [Id={@Id}]", loginData.Id);
                return new Result(Status.WrongPassword, null);
            }

            var userSnippet = await DbContext.Users
                .Where(x => x.Id == loginData.Id)
                .ProjectTo<UserSnippetDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            if (userSnippet is null)
            {
                return new Result(Status.UserNotFound, null);
            }

            string? jwt = JwtService.GenerateUserLoginJWT(loginData.Id, out int expiresInSeconds);
            var userLogin = new UserLoginDto(userSnippet, jwt, expiresInSeconds);

            Logger.LogInformation("Login success [Id={id}]", loginData.Id);

            return new Result(Status.Success, userLogin);
        }
    }
}
