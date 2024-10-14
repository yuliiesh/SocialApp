namespace SocialApp.Common.Posts.Get;

public class GetPostsResponse
{
    public IReadOnlyCollection<PostDto> Posts { get; set; }
}