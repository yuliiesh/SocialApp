using Microsoft.AspNetCore.Components;
using SocialApp.Client.Authorization;
using SocialApp.Client.Extensions;
using SocialApp.Client.Pages;
using SocialApp.Client.Services;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Components.Base;

public partial class SidebarComponent : LoadingComponent
{
    private ProfileDto _profile;

    [Inject] private UserAuthenticationStateProvider UserAuthenticationStateProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }
    [Inject] private IImageService ImageService { get; set; }

    [Parameter] public HomePage.Tab Tab { get; set; }
    [Parameter] public EventCallback<HomePage.Tab> OnTabChanged { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await HandleLoadAction(async () =>
        {
            _profile = await LocalStorage.GetProfile();
        });
    }

    private void Logout()
    {
        UserAuthenticationStateProvider.MarkUserAsLoggedOut();
        NavigationManager.NavigateTo("authorize");
    }

    private void ChangeTab(HomePage.Tab tab)
    {
        if (OnTabChanged.HasDelegate)
        {
            OnTabChanged.InvokeAsync(tab);
        }
    }
}