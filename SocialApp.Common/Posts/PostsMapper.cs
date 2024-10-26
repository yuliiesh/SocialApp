using SocialApp.Common.Posts.Create;
using SocialApp.Data.Models;

namespace SocialApp.Common.Posts;

public static class PostsMapper
{
    public static CreatePostResponse ToCreatePostResponse(this PostModel model, ProfileModel profile) =>
        new()
        {
            Id = model.Id,
            UserId = model.UserId,
            Title = model.Title,
            Content = model.Content,
            CreatedAt = model.CreatedAt,
            FirstName = profile?.FirstName,
            LastName = profile?.LastName,
        };

    public static PostModel ToPostModel(this CreatePostRequest request) =>
        new()
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            CreatedAt = DateTime.Now,
            Title = request.Title,
            UserId = request.UserId,
        };

    public static PostDto ToDto(this PostModel model, ProfileModel profile) =>
        new()
        {
            Id = model.Id,
            UserId = model.UserId,
            Title = model.Title,
            Content = model.Content,
            CreatedAt = model.CreatedAt,
            DeletedAt = model.DeletedAt,
            UpdatedAt = model.UpdatedAt,
            FirstName = profile?.FirstName,
            LastName = profile?.LastName,
        };
}