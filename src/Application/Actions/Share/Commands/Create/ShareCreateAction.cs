using MediatR;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Command;
public class ShareCreateAction
{
    public class Command : IRequest<Result>
    {
        public UserId OwnerId { get; }

        public Sharename Name { get; }

        public bool IsPrivate { get; }

        [MaxLength(DomainConstraints.ShareDescriptionLengthMaximum)]
        public string Description { get; }

        [MaxLength(DomainConstraints.ShareReadmeLengthMaximum)]
        public string Readme { get; }

        public Command(UserId ownerId, Sharename name, bool isPrivate, string description, string readme)
        {
            OwnerId = ownerId;
            Name = name;
            IsPrivate = isPrivate;
            Description = description;
            Readme = readme;
        }
    }

    public enum Status : byte
    {
        Success,
        NameTaken,
    }

    public record Result(Status Status, ShareId? ShareId = null);

    [Authorize]
    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;
        private readonly ISecretService SecretService;

        public Handler(IShareContext dbContext,
            ISecretService secretService, IAuthorizer authorizer)
        {
            DbContext = dbContext;
            SecretService = secretService;
            Authorizer = authorizer;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var shareSecret = SecretService.GenerateShareSecret();
            var share = Share.Create(request.OwnerId, request.Name, request.IsPrivate, request.Description, request.Readme, shareSecret);

            await Authorizer.EnsureAuthorizationAsync(share, ShareCommandOperation.Create, cancellationToken);

            DbContext.Shares.Add(share);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.DuplicateIndex, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success, share.Id),
                DbStatus.DuplicateIndex => new Result(
                    saveResult.IsConflictingIndex(typeof(Share), nameof(Share.OwnerId), nameof(Share.Name))
                       ? Status.NameTaken
                       : throw new UnhandledIndexConflictException(saveResult)),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

