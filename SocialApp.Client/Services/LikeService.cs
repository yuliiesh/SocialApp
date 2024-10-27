using System.Net.Http.Json;
using SocialApp.Common.Comments;

namespace SocialApp.Client.Services;

public interface ILikeService
{
    Task<int> GetLikesCountForPost(Guid postId);
    Task LikePost(Guid postId, Guid userId);
    Task UnlikePost(Guid postId, Guid userId);
}

public class LikeService : ILikeService
{
    private readonly HttpClient _httpClient;

    public LikeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetLikesCountForPost(Guid postId)
    {
        var count = await _httpClient.GetFromJsonAsync<int>($"api/likes/count?postId={postId}");
        return count;
    }

    public async Task LikePost(Guid postId, Guid userId) =>
        await _httpClient.PostAsync($"api/likes/post?userId={userId}&postId={postId}", default);

    public async Task UnlikePost(Guid postId, Guid userId) =>
        await _httpClient.DeleteAsync($"api/likes/post?userId={userId}&postId={postId}");
}