using System.Text.Json;
using Microsoft.AspNetCore.Components;
using SocialApp.Client.Extensions;
using SocialApp.Client.Services;
using SocialApp.Common.Comments;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Components.Comment;

public partial class CommentsComponent : ComponentBase
{
    private string _newComment = string.Empty;

    [Inject] private ICommentService CommentService { get; set; }

    [Inject] private ILocalStorageService LocalStorage { get; set; }

    [Parameter] public ICollection<CommentDto> Comments { get; set; }

    [Parameter] public Guid PostId { get; set; }

    [Inject] private ILogger<CommentsComponent> Logger { get; set; }

    private async Task SubmitComment()
    {
        if (string.IsNullOrWhiteSpace(_newComment))
        {
            return;
        }

        var profile = await LocalStorage.GetProfile();

        var newComment = new CommentDto
        {
            Content = _newComment,
            CreatedAt = DateTime.Now,
            PostId = PostId,
            ProfileInfo = new()
            {
                UserId = await LocalStorage.GetUserId(),
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                ProfilePicture = profile.ProfilePicture
            }
        };

        Comments.Add(newComment);
        _newComment = string.Empty;

        await CommentService.AddComment(newComment);
    }
}