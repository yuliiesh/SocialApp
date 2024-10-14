using Microsoft.AspNetCore.Components;
using SocialApp.Common.Posts;

namespace SocialApp.Client.Components.Post;

public partial class PostOverview : ComponentBase
{
    private bool _showCommnets = false;

    [Parameter] public PostDto Post { get; set; }

    private void ToggleShowCommnets() => _showCommnets = !_showCommnets;
}