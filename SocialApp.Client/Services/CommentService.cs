using System.Net.Http.Json;
using SocialApp.Common.Comments;

namespace SocialApp.Client.Services;

public interface ICommentService
{
    Task<ICollection<CommentDto>> GetComments(Guid postId);
    Task<int> GetCommentsCount(Guid postId);
    Task AddComment(CommentDto comment);
}

public class CommentService : ICommentService
{
    private readonly HttpClient _httpClient;

    public CommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ICollection<CommentDto>> GetComments(Guid postId) =>
        await _httpClient.GetFromJsonAsync<ICollection<CommentDto>>($"/api/comments?postId={postId}");

    public async Task<int> GetCommentsCount(Guid postId) =>
        await _httpClient.GetFromJsonAsync<int>($"/api/comments/count?postId={postId}");

    public async Task AddComment(CommentDto comment) =>
        await _httpClient.PostAsJsonAsync($"/api/comments/post/{comment.PostId}", comment);
}