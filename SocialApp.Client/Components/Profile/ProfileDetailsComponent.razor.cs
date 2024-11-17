using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SocialApp.Client.Extensions;
using SocialApp.Client.Services;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Components.Profile;

public partial class ProfileDetailsComponent : ComponentBase
{
    private bool _owner;
    private bool _isFriend;

    [Inject] private ILocalStorageService LocalStorage { get; set; }

    [Parameter] public ProfileDto Profile { get; set; }
    [Parameter] public string ProfileImagePreview { get; set; }
    [Parameter] public EventCallback<InputFileChangeEventArgs> OnProfilePhotoChange { get; set; }
    [Parameter] public EventCallback AddToFriend { get; set; }
    [Parameter] public EventCallback RemoveFromFriends { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        var currentUser = await LocalStorage.GetProfile();
        _owner = Profile.UserId == currentUser.UserId;
        _isFriend = currentUser.Friends.Contains(Profile.UserId);
    }
}