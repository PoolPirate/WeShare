namespace WeShare.Domain.Entities;
public enum PostSendFailureType
{
    MessagerDiscord = 200,
    Webhook = 400,

    InternalError = 1000,
}

