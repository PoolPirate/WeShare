using WeShare.Domain.Common;

namespace WeShare.Application.Entities;
public class PostMetadata
{
    public ByteCount HeadersSize { get; }
    public ByteCount PayloadSize { get; }

    public PostMetadata(ByteCount headerSize, ByteCount payloadSize)
    {
        HeadersSize = headerSize;
        PayloadSize = payloadSize;
    }
}

