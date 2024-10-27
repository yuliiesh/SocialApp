using SocialApp.Common.Base;

namespace SocialApp.Common.Posts.Create;

public class CreatePostResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public ProfileInfo ProfileInfo { get; set; }
}