namespace WeShare.Domain.Entities;
public class PostContent
{
    public IDictionary<string, string[]> Headers { get; }
    public byte[] Payload { get; }

    public PostContent(IDictionary<string, string[]> headers, byte[] payload)
    {
        Headers = headers;
        Payload = payload;
    }
}

