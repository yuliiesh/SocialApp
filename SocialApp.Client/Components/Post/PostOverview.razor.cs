using Microsoft.AspNetCore.Components;
using SocialApp.Client.Services;
using SocialApp.Common.Comments;
using SocialApp.Common.Posts;

namespace SocialApp.Client.Components.Post;

public partial class PostOverview : ComponentBase
{
    private bool _showCommnets = false;

    private IReadOnlyCollection<CommentDto> _comments;

    [Inject] private ICommentService CommentService { get; set; }

    [Parameter] public PostDto Post { get; set; }

    private async Task ToggleShowComments()
    {
        _showCommnets = !_showCommnets;
        if (_showCommnets)
        {
            await GetComments();
        }
    }

    private async Task GetComments()
    {
        if (_comments is null)
        {
            _comments = await CommentService.GetComments(Post.Id, CancellationToken.None);
        }
    }
}