using SocialApp.Api.Posts.Create;
using SocialApp.Common.Posts;
using SocialApp.Data.Models;

namespace SocialApp.Api.Posts;

public static class PostsMapper
{
    public static CreatePostResponse ToCreatePostResponse(this PostModel model) =>
        new()
        {
            Id = model.Id,
            UserId = model.UserId,
            Title = model.Title,
            Content = model.Content,
            CreatedAt = model.CreatedAt,
        };

    public static PostModel ToPostModel(this CreatePostRequest request) =>
        new()
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            CreatedAt = DateTime.Now,
            Title = request.Title,
        };

    public static PostDto ToDto(this PostModel model) =>
        new()
        {
            Id = model.Id,
            UserId = model.UserId,
            Title = model.Title,
            Content = model.Content,
            CreatedAt = model.CreatedAt,
            DeletedAt = model.DeletedAt,
            UpdatedAt = model.UpdatedAt,
        };
}