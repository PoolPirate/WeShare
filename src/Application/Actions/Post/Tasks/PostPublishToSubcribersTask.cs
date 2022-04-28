using MediatR;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Tasks;
public class PostPublishToSubcribersTask
{
    public class Command : IRequest<Result>
    {
        public PostId PostId { get; }
        [EnumDataType(typeof(SubscriptionType))]
        public SubscriptionType SubscriptionType { get; }

        public Command(PostId postId, SubscriptionType subscriptionType)
        {
            PostId = postId;
            SubscriptionType = subscriptionType;
        }
    }

    public enum Status : byte
    {
        Success,
    }

    public record Result(Status Status);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;

        public Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

