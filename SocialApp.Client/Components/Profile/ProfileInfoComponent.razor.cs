using Microsoft.AspNetCore.Components;
using SocialApp.Client.Extensions;
using SocialApp.Client.Services;
using SocialApp.Common.Posts;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Components.Profile;

public partial class ProfileInfoComponent : ComponentBase
{
    private bool _isFriend { get; set; }

    [Parameter] public ProfileDto Profile { get; set; }
    [Parameter] public bool Loading { get; set; }
    [Parameter] public EventCallback<PostDto> PostCreated { get; set; }
    [Parameter] public EventCallback<PostDto> PostDeleted { get; set; }
    [Parameter] public ICollection<PostDto> Posts { get; set; }

    [Inject] private ILocalStorageService LocalStorage { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        var currentUserId = await LocalStorage.GetUserId();
        _isFriend = currentUserId != Profile.UserId;
    }
}