using System.Net.Http.Json;
using SocialApp.Common.Comments;

namespace SocialApp.Client.Services;

public interface ILikeService
{
    Task<int> GetLikesCountForPost(Guid postId, CancellationToken cancellationToken = default);
}

public class LikeService : ILikeService
{
    private readonly HttpClient _httpClient;

    public LikeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<int> GetLikesCountForPost(Guid postId, CancellationToken cancellationToken = default)
    {
        var count = await _httpClient.GetFromJsonAsync<int>($"api/likes/count?postId={postId}", cancellationToken: cancellationToken);
        return count;
    }
}