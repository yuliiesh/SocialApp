using Microsoft.AspNetCore.Components;
using SocialApp.Client.Components.Base;
using SocialApp.Client.Services;
using SocialApp.Common.Friends;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Components.Friends;

public partial class FriendsComponent : LoadingComponent
{
    private IReadOnlyCollection<FriendDto> _friends;
    private string SearchText { get; set; } = string.Empty;

    [Inject] private IFriendService FriendService { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await HandleLoadAction(async () =>
        {
            var userId = (await LocalStorage.GetItem<ProfileDto>("profile")).UserId;
            _friends = await FriendService.GetFriends(userId);
        });
    }
}