using Microsoft.AspNetCore.Components;
using SocialApp.Client.Components.Base;
using SocialApp.Client.Services;
using SocialApp.Common.Posts;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Pages;

public partial class PostsPage : LoadingComponent
{
    private IList<PostDto> _posts;
    private ProfileDto _profile;

    [Inject] private IPostService PostService { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await HandleLoadAction(async () =>
        {
            _posts = await PostService.GetAllPosts(CancellationToken.None);
            _profile = await LocalStorage.GetItem<ProfileDto>("profile");
        });
    }

    private void PostCreated(PostDto post)
    {
        _posts.Insert(0, post);
        StateHasChanged();
    }
}