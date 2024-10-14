using System.Net.Http.Json;
using SocialApp.Common.Posts;
using SocialApp.Common.Posts.Get;

namespace SocialApp.Client.Services;

public interface IPostService
{
    Task<IReadOnlyCollection<PostDto>> GetAllPosts(CancellationToken cancellationToken);
}

public class PostService : IPostService
{
    private readonly HttpClient _httpClient;

    public PostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<PostDto>> GetAllPosts(CancellationToken cancellationToken) =>
        (await _httpClient.GetFromJsonAsync<GetPostsResponse>("api/posts", cancellationToken)).Posts;
}