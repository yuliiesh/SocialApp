using SocialApp.Common.Comments;
using SocialApp.Data.Models;

namespace SocialApp.Api.Comments;

public static class CommentsMapper
{
    public static CommentDto ToDto(this CommentModel model) =>
        new()
        {
            Content = model.Content,
            CreatedAt = model.CreatedAt,
            PostId = model.PostId,
        };
}