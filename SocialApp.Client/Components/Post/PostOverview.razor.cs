using Microsoft.AspNetCore.Components;
using SocialApp.Client.Services;
using SocialApp.Common.Comments;
using SocialApp.Common.Posts;

namespace SocialApp.Client.Components.Post;

public partial class PostOverview : ComponentBase
{
    private bool _loading;
    private bool _showComments = false;

    private IReadOnlyCollection<CommentDto> _comments;

    private int _commentsCount = 0;
    private int _likesCount = 0;

    [Inject] private ICommentService CommentService { get; set; }
    [Inject] private ILikeService LikeService { get; set; }
    [Parameter] public PostDto Post { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        _commentsCount = await CommentService.GetCommentsCount(Post.Id, default);
        _likesCount = await LikeService.GetLikesCountForPost(Post.Id, default);
        _loading = false;
    }

    private async Task ToggleShowComments()
    {
        _showComments = !_showComments;
        if (_showComments)
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