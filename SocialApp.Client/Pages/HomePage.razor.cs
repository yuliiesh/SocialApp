using System.Text.Json;
using Microsoft.AspNetCore.Components;
using SocialApp.Client.Components.Base;
using SocialApp.Client.Extensions;
using SocialApp.Client.Services;
using SocialApp.Common.Posts;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Pages;

public partial class HomePage : LoadingComponent
{
    private IList<PostDto> _posts;
    private ProfileDto _profile;
    private Tab _tab;

    [Inject] private IPostService PostService { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }
    [Inject] private ILogger<HomePage> Logger { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _tab = Tab.Posts;
        await HandleLoadAction(async () =>
        {
            _posts = await PostService.GetAllPosts(CancellationToken.None);
            _profile = await LocalStorage.GetProfile();
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

    private void TabChanged(Tab tab)
    {
        _tab = tab;
        StateHasChanged();
    }

    public enum Tab
    {
        Posts,
        Friends
    }
}