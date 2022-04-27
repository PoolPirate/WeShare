﻿using WeShare.Application.Common.Mappings;
using WeShare.Domain.Common;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;
public class PostMetadataDto : IMapFrom<Post>
{
    public PostId Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public ByteCount HeadersSize { get; set; }
    public ByteCount PayloadSize { get; set; }
}

