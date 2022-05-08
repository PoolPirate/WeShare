using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WeShare.Application.Entities;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class PostSubmitAction
{
    public class Command : IRequest<Result>
    {
        public ShareSecret ShareSecret { get; }

        public IDictionary<string, string[]> Headers { get; }
        public Stream Payload { get; }

        public Command(ShareSecret shareSecret, IDictionary<string, string[]> headers, Stream payload)
        {
            ShareSecret = shareSecret;
            Headers = headers;
            Payload = payload;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNotFound,
    }

    public record Result(Status Status);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IMapper Mapper;
        private readonly IPostStorage PostStorage;
        private readonly IPostProcessor PostProcessor;

        public Handler(IShareContext dbContext, IMapper mapper, IPostStorage postStorage, IPostProcessor postProcessor)
        {
            DbContext = dbContext;
            Mapper = mapper;
            PostStorage = postStorage;
            PostProcessor = postProcessor;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var context = await DbContext.Shares
                .Where(x => x.Secret == request.ShareSecret)
                .ProjectTo<PostProcessingContext>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (context is null)
            {
                return new Result(Status.ShareNotFound);
            }

            var (processedHeaders, processedPayload) = await PostProcessor.PreProcessAsync(request.Headers, request.Payload, context);

            using var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);

            var post = Post.Create(context.ShareId);
            DbContext.Posts.Add(post);

            await DbContext.SaveChangesAsync(cancellationToken: cancellationToken);

            var metadata = await PostStorage.StoreAsync(post.Id, processedHeaders, processedPayload, cancellationToken);

            try
            {
                post.SetMetadata(metadata.HeadersSize, metadata.PayloadSize);
                await DbContext.SaveChangesAsync(transactionScope: transactionScope, cancellationToken: cancellationToken);
            }
            catch
            {
                await PostStorage.DeleteAsync(post.Id);
                throw;
            }

            return new Result(Status.Success);
        }
    }
}

