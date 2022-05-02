namespace WeShare.Application.Actions.Queries;
public enum UserQueryOperation : byte
{
    ReadSnippet,
    ReadLikedShares,
    ReadSubscriptions,
    ReadPublicShareSnippets,
    ReadProfile,
    ReadAccount,
}