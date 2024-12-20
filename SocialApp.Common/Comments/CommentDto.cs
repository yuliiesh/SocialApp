﻿using SocialApp.Common.Base;

namespace SocialApp.Common.Comments;

public class CommentDto
{
    public string Content { get; set; }
    public Guid PostId { get; set; }
    public DateTime CreatedAt { get; set; }

    public ProfileInfo ProfileInfo { get; set; }
}