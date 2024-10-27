using Microsoft.AspNetCore.Components;
using SocialApp.Client.Components.Base;
using SocialApp.Client.Extensions;
using SocialApp.Client.Services;
using SocialApp.Common.Comments;
using SocialApp.Common.Posts;

namespace SocialApp.Client.Components.Post;

public partial class PostOverview : LoadingComponent
{
    private bool _showComments = false;
    private bool _haveLike = false;
    private bool _isMenuOpen = false;
    private Guid _userId;
    private string _newComment;

    private ICollection<CommentDto> _comments;

    private int _commentsCount = 0;
    private int _likesCount = 0;

    [Inject] private ICommentService CommentService { get; set; }
    [Inject] private ILikeService LikeService { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }

    [Parameter] public PostDto Post { get; set; }
    [Parameter] public EventCallback<PostDto> OnPostDeleted { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await HandleLoadAction(async () =>
        {
            _commentsCount = await CommentService.GetCommentsCount(Post.Id);
            _likesCount = Post.UserLikes.Count;
            _userId = await LocalStorage.GetUserId();
            _haveLike = Post.UserLikes?.Contains(_userId) ?? false;
        });
    }

    private async Task ToggleShowComments()
    {
        _showComments = !_showComments;
        if (_showComments)
        {
            await GetComments();
        }
        StateHasChanged();
    }

    private async Task GetComments()
    {
        _comments ??= await CommentService.GetComments(Post.Id);
    }

    private async Task HandleLike()
    {
        if (Post.UserLikes.Contains(_userId))
        {
            await LikeService.UnlikePost(Post.Id, _userId);
            Post.UserLikes.Remove(_userId);
        }
        else
        {
            await LikeService.LikePost(Post.Id, _userId);
            Post.UserLikes.Add(_userId);
        }
        _likesCount = Post.UserLikes.Count;
        _haveLike = Post.UserLikes?.Contains(_userId) ?? false;
        StateHasChanged();
    }

    private async Task ToggleMenu()
    {
        var currentUserId = await LocalStorage.GetUserId();
        if (currentUserId == Post.UserId)
        {
            _isMenuOpen = !_isMenuOpen;
        }
    }

    private void DeletePost()
    {
        if (OnPostDeleted.HasDelegate)
        {
            OnPostDeleted.InvokeAsync(Post);
        }
        _isMenuOpen = false;
    }
}