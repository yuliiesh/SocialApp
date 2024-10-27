using Microsoft.AspNetCore.Components;
using SocialApp.Client.Components.Profile;
using SocialApp.Client.Extensions;
using SocialApp.Client.Services;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Pages.Profile;

public partial class FriendsPage : BaseProfileComponent
{
    private bool _isFriend;

    [Inject] private IProfileService ProfileService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private IFriendService FriendService { get; set; }

    [Parameter] public string Username { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await HandleLoadAction(async () =>
        {
            var profile = await LocalStorage.GetProfile();
            if (profile.Username.Equals(Username, StringComparison.OrdinalIgnoreCase))
            {
                Navigation.NavigateTo("/Profile", replace: true);
            }

            await base.OnInitializedAsync();
            _isFriend = await IsFriend();
        });
    }

    protected override async Task LoadProfile()
    {
        _profile = await ProfileService.GetByUsername(Username);
    }

    private async Task AddToFriend()
    {
        var currentUser = await LocalStorage.GetProfile();
        var currentUserId = currentUser.UserId;
        var friendId = _profile.UserId;

        await FriendService.AddFriend(currentUserId, friendId);

        currentUser.Friends ??= [];
        currentUser.Friends.Add(friendId);

        await LocalStorage.SetItem("profile", currentUser);

        _profile.Friends ??= [];
        _profile.Friends.Add(currentUserId);

        _isFriend = await IsFriend();
        StateHasChanged();
    }

    private async Task RemoveFromFriends()
    {
        var currentUser = await LocalStorage.GetProfile();
        var currentUserId = currentUser.UserId;
        var friendId = _profile.UserId;

        await FriendService.RemoveFriend(currentUserId, friendId);

        currentUser.Friends.Remove(friendId);
        await LocalStorage.SetItem("profile", currentUser);

        _profile.Friends.Remove(currentUserId);
        _isFriend = await IsFriend();
        StateHasChanged();
    }

    private async Task<bool> IsFriend()
    {
        var currentUser = (await LocalStorage.GetItem<ProfileDto>("profile"));

        return currentUser.Friends.Contains(_profile.UserId);
    }
}