using MediatR;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Tasks;
public class PostPublishTask
{
    public class Command<TSubscription> : IRequest
        where TSubscription : Subscription
    {
        public PostId PostId { get; }

        public Command(PostId postId)
        {
            PostId = postId;
        }
    }

    public class Handler<TSubscription> : IRequestHandler<Command<TSubscription>>
        where TSubscription : Subscription
    {
        private readonly PostPublisher<TSubscription> Publisher;

        public Handler(PostPublisher<TSubscription> publisher)
        {
            Publisher = publisher;
        }

        public async Task<Unit> Handle(Command<TSubscription> request, CancellationToken cancellationToken)
        {
            await Publisher.PublishToSubscribersAsync(request.PostId, cancellationToken);
            return Unit.Value;
        }
    }
}

