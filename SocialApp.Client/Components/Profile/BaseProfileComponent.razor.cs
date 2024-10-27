using Microsoft.AspNetCore.Components;
using SocialApp.Client.Components.Base;
using SocialApp.Client.Services;
using SocialApp.Common.Posts;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Components.Profile;

public abstract class BaseProfileComponent : LoadingComponent
{
    protected IList<PostDto> _posts;
    protected ProfileDto _profile;

    [Inject] protected IPostService PostService { get; set; }
    [Inject] protected ILocalStorageService LocalStorage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await HandleLoadAction(async () =>
        {
            await LoadProfile();
            _posts = await PostService.GetAllPosts(_profile.UserId, CancellationToken.None);
        });
    }

    protected abstract Task LoadProfile();
}