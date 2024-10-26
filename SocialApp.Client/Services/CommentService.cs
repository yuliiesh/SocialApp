using System.Net.Http.Json;
using SocialApp.Common.Comments;

namespace SocialApp.Client.Services;

public interface ICommentService
{
    Task<IReadOnlyCollection<CommentDto>> GetComments(Guid postId, CancellationToken cancellationToken);
    Task<int> GetCommentsCount(Guid postId, CancellationToken cancellationToken);
}

public class CommentService : ICommentService
{
    private readonly HttpClient _httpClient;

    public CommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<CommentDto>> GetComments(Guid postId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CommentDto>>($"/api/comments?postId={postId}", cancellationToken);
        return response;
    }

    public async Task<int> GetCommentsCount(Guid postId, CancellationToken cancellationToken)
    {
        return await _httpClient.GetFromJsonAsync<int>($"/api/comments/count?postId={postId}", cancellationToken);
    }
}