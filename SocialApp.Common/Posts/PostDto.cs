﻿using SocialApp.Common.Base;

namespace SocialApp.Common.Posts;

public class PostDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ProfileInfo ProfileInfo { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }

    public HashSet<Guid> UserLikes { get; set; }
}