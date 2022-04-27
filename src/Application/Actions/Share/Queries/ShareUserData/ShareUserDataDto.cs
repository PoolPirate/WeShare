namespace WeShare.Application.DTOs;

public class ShareUserDataDto
{
    public bool Liked { get; private set; }

    public ShareUserDataDto(bool liked)
    {
        Liked = liked;
    }
}

