using Microsoft.AspNetCore.Components;
using SocialApp.Common.Comments;

namespace SocialApp.Client.Components.Comment;

public partial class CommentComponent : ComponentBase
{
    [Parameter] public CommentDto Comment { get; set; }
}