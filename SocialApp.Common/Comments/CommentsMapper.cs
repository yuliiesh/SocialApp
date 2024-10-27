using SocialApp.Data.Models;

namespace SocialApp.Common.Comments;

public static class CommentsMapper
{
    public static CommentDto ToDto(this CommentModel model, ProfileModel profile) =>
        new()
        {
            Content = model.Content,
            CreatedAt = model.CreatedAt,
            PostId = model.PostId,
            ProfileInfo = new()
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                UserId = profile.UserId,
                Username = profile.Username,
            }
        };
}