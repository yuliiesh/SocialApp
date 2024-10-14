using Microsoft.AspNetCore.Components;
using SocialApp.Client.Authorization;

namespace SocialApp.Client.Components.Base;

public partial class SidebarComponent : ComponentBase
{
    [Inject] UserAuthenticationStateProvider UserAuthenticationStateProvider { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    private void Logout()
    {
        UserAuthenticationStateProvider.MarkUserAsLoggedOut();
        NavigationManager.NavigateTo("authorize");
    }
}