namespace WeShare.Application.DTOs;

public class ShareDataDto
{
    public ShareInfoDto ShareInfo { get; private set; }
    public UserSnippetDto OwnerSnippet { get; private set; }

    public ShareDataDto(ShareInfoDto shareInfo, UserSnippetDto owner)
    {
        ShareInfo = shareInfo;
        OwnerSnippet = owner;
    }
}
