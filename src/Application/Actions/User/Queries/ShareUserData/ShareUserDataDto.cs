namespace WeShare.Application.DTOs;

public class ShareUserDataDto
{
    public bool Liked { get; private set; }
    public bool Subscribed { get; private set; }

    public ShareUserDataDto(bool liked, bool subscribed)
    {
        Liked = liked;
        Subscribed = subscribed;
    }
}

