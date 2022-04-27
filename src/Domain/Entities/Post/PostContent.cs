using Microsoft.Extensions.Primitives;

namespace WeShare.Domain.Entities;
public class PostContent
{
    public IReadOnlyDictionary<string, StringValues> Headers { get; }
    public Stream Payload { get; }

    public PostContent(IReadOnlyDictionary<string, StringValues> headers, Stream payload)
    {
        Headers = headers;
        Payload = payload;
    }
}

