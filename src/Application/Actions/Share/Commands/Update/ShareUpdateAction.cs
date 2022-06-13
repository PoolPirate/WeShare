using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class ShareUpdateAction
{
    public class Command : IRequest<Result>
    {
        public ShareId ShareId { get; }

        [MinLength(DomainConstraints.ShareNameLengthMinimum)]
        [MaxLength(DomainConstraints.ShareNameLengthMaximum)]
        public ShareName? Name { get; }

        [MaxLength(DomainConstraints.ShareDescriptionLengthMaximum)]
        public string? Description { get; }

        [MaxLength(DomainConstraints.ShareReadmeLengthMaximum)]
        public string? Readme { get; }

        public Command(ShareId shareId, ShareName? name, string? description, string? readme)
        {
            ShareId = shareId;
            Name = name;
            Description = description;
            Readme = readme;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNameTaken,
        ShareNotFound
    }

    public record Result(Status Status);

    [Authorize]
    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;

        public Handler(IShareContext dbContext, IAuthorizer authorizer)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var share = await DbContext.Shares
                .Where(x => x.Id == request.ShareId)
                .SingleOrDefaultAsync(cancellationToken);

            if (share is null)
            {
                return new Result(Status.ShareNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(share, ShareCommandOperation.Update, cancellationToken);

            ApplyUpdates(share, request);

            var saveResult = await DbContext.SaveChangesAsync(
                DbStatus.DuplicateIndex | DbStatus.ConcurrencyEntryDeleted, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.DuplicateIndex => new Result(
                    saveResult.IsConflictingIndex(typeof(Share), nameof(Share.Id), nameof(Share.Name))
                        ? Status.ShareNameTaken
                        : throw new UnhandledIndexConflictException(saveResult)
                    ),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.ShareNotFound),
                _ => throw new UnhandledDbStatusException(saveResult)
            };
        }

        private static void ApplyUpdates(Share share, Command request)
        {
            if (request.Name.HasValue)
            {
                share.Name = request.Name.Value;
            }
            if (request.Description is not null)
            {
                share.Description = request.Description;
            }
            if (request.Readme is not null)
            {
                share.Readme = request.Readme;
            }
        }
    }
}

