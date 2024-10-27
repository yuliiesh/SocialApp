using Microsoft.AspNetCore.Components;
using SocialApp.Client.Components.Base;
using SocialApp.Client.Extensions;
using SocialApp.Client.Services;
using SocialApp.Common.Posts;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Pages.Profile;

public partial class ProfilePage : LoadingComponent
{
    private IList<PostDto> _posts;
    private ProfileDto _profile;

    [Inject] private IPostService PostService { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await HandleLoadAction(async () =>
        {
            _profile = await LocalStorage.GetProfile();
            _posts = await PostService.GetAllPosts(_profile.UserId, CancellationToken.None);
        });
    }

    private void PostCreated(PostDto post)
    {
        _posts.Insert(0, post);
        StateHasChanged();
    }

    private async Task PostDeleted(PostDto post)
    {
        await PostService.DeletePost(post.Id);
        _posts.Remove(post);
        StateHasChanged();
    }
}