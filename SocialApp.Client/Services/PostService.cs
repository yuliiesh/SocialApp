using System.Net.Http.Json;
using SocialApp.Common.Posts;
using SocialApp.Common.Posts.Create;
using SocialApp.Common.Posts.Get;

namespace SocialApp.Client.Services;

public interface IPostService
{
    Task<IList<PostDto>> GetAllPosts(CancellationToken cancellationToken);
    Task<IList<PostDto>> GetAllPosts(Guid userId, CancellationToken cancellationToken);
    Task<PostDto>  CreatePost(CreatePostRequest request, CancellationToken cancellationToken);
    Task DeletePost(Guid postId, CancellationToken cancellationToken = default);

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

    public async Task<IList<PostDto>> GetAllPosts(Guid userId, CancellationToken cancellationToken) =>
        (await _httpClient.GetFromJsonAsync<GetPostsResponse>($"api/posts/{userId}", cancellationToken)).Posts;

    public async Task<PostDto> CreatePost(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/posts", request, cancellationToken);
        return await response.Content.ReadFromJsonAsync<PostDto>(cancellationToken: cancellationToken);
    }

    public Task DeletePost(Guid postId, CancellationToken cancellationToken) =>
        _httpClient.DeleteAsync($"api/posts/{postId}", cancellationToken);
}