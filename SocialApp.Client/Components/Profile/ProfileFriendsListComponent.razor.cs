using Microsoft.AspNetCore.Components;
using SocialApp.Client.Components.Base;
using SocialApp.Client.Services;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Components.Profile;

public partial class ProfileFriendsListComponent : LoadingComponent
{
    private IEnumerable<ProfileDto> _friends { get; set; }

    [Inject] private IImageService ImageService { get; set; }
    [Inject] private IProfileService ProfileService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }

    [Parameter] public ProfileDto Profile { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        await HandleLoadAction(async () => { _friends = await ProfileService.Get(Profile.Friends); });
    }

    private void ToFriend(string username)
    {
        Navigation.NavigateTo($"/profile/{username}");
    }
}