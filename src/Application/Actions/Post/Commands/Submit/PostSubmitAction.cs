using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using WeShare.Application.Entities;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Command;
public class PostSubmitAction
{
    public class Command : IRequest<Result>
    {
        public ShareSecret ShareSecret { get; }

        public Dictionary<string, StringValues> Headers { get; }
        public Stream Payload { get; }

        public Command(ShareSecret shareSecret, Dictionary<string, StringValues> headers, Stream payload)
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

            var content = new PostContent(request.Headers, request.Payload);
            var metaData = await PostProcessor.PreProcessAsync(content, context);

            using var transaction = await DbContext.BeginTransactionAsync(cancellationToken);

            var post = Post.Create(metaData.HeadersSize, metaData.PayloadSize, context.ShareId);
            DbContext.Posts.Add(post);

            var saveResult = await DbContext.SaveChangesAsync(cancellationToken: cancellationToken);

            await PostStorage.StoreAsync(post.Id, content, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new Result(Status.Success);
        }
    }
}

