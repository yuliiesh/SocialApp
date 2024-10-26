using System.Net.Http.Json;
using SocialApp.Common.Posts;
using SocialApp.Common.Posts.Create;
using SocialApp.Common.Posts.Get;

namespace SocialApp.Client.Services;

public interface IPostService
{
    Task<IList<PostDto>> GetAllPosts(CancellationToken cancellationToken);

    Task<PostDto>  CreatePost(CreatePostRequest request, CancellationToken cancellationToken);
}

public class PostService : IPostService
{
    private readonly HttpClient _httpClient;

    public PostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IList<PostDto>> GetAllPosts(CancellationToken cancellationToken) =>
        (await _httpClient.GetFromJsonAsync<GetPostsResponse>("api/posts", cancellationToken)).Posts;

    public async Task<PostDto> CreatePost(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/posts", request, cancellationToken);
        return await response.Content.ReadFromJsonAsync<PostDto>(cancellationToken: cancellationToken);
    }

}