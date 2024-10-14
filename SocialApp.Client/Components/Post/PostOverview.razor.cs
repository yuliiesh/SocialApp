using Microsoft.AspNetCore.Components;
using SocialApp.Common.Posts;

namespace SocialApp.Client.Components.Post;

public partial class PostOverview : ComponentBase
{
    [Parameter]
    public PostDto Post { get; set; }

}