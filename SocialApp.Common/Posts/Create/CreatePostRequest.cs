namespace SocialApp.Common.Posts.Create;

public class CreatePostRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid UserId { get; set; }
}