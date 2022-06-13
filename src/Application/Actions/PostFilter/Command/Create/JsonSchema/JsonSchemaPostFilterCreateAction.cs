using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class JsonSchemaPostFilterCreateAction
{
    public class Command : IRequest<Result>
    {
        public ShareId ShareId { get; }
        public PostFilterName Name { get; }
        [Required]
        public string Schema { get; }

        public Command(ShareId shareId, PostFilterName name, string schema)
        {
            ShareId = shareId;
            Name = name;
            Schema = schema;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNotFound,
        MalformedSchema,
    }

    public record Result(Status Status, PostFilter? PostFilter = null);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IJsonSchemaValidator JsonSchemaValidator;
        private readonly IAuthorizer Authorizer;

        public Handler(IShareContext dbContext, IJsonSchemaValidator jsonSchemaValidator, IAuthorizer authorizer)
        {
            DbContext = dbContext;
            JsonSchemaValidator = jsonSchemaValidator;
            Authorizer = authorizer;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var postFilter = new JsonSchemaPostFilter(request.ShareId, request.Name, request.Schema);
            await Authorizer.EnsureAuthorizationAsync(postFilter, PostFilterCommandOperation.Create, cancellationToken);

            if (!await DbContext.Shares.AnyAsync(x => x.Id == request.ShareId, cancellationToken))
            {
                return new Result(Status.ShareNotFound);
            }
            if(!await JsonSchemaValidator.IsValidJsonSchema(request.Schema, cancellationToken))
            {
                return new Result(Status.MalformedSchema);
            }

            DbContext.PostFilters.Add(postFilter);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success, postFilter),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.ShareNotFound),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}
