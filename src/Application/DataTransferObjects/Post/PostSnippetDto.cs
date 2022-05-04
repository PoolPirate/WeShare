using WeShare.Application.Common.Mappings;
using WeShare.Domain.Common;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;
public class PostSnippetDto : IMapFrom<Post>
{
    public long Id { get; set; }
    public long ShareId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long HeadersSize { get; set; }
    public long PayloadSize { get; set; }
}

