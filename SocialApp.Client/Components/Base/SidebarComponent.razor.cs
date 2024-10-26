using Microsoft.AspNetCore.Components;
using SocialApp.Client.Authorization;
using SocialApp.Client.Services;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Components.Base;

public partial class SidebarComponent : LoadingComponent
{
    private ProfileDto _profile;

    [Inject] private UserAuthenticationStateProvider UserAuthenticationStateProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await HandleLoadAction(async () =>
        {
            _profile = await LocalStorage.GetItem<ProfileDto>("profile");
        });
    }

    private void Logout()
    {
        UserAuthenticationStateProvider.MarkUserAsLoggedOut();
        NavigationManager.NavigateTo("authorize");
    }
}