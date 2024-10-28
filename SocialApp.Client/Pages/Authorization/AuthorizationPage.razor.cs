using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using SocialApp.Client.Authorization;
using SocialApp.Common.Authorization;

namespace SocialApp.Client.Pages.Authorization;

public partial class AuthorizationPage : ComponentBase
{
    [Inject] private HttpClient Client { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private UserAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private bool _loginAction = true;

    private async Task Login(LoginRequest loginRequest)
    {
        var response = await Client.PostAsJsonAsync("api/auth/login", loginRequest);
        if (response.IsSuccessStatusCode)
        {
            await AuthenticationStateProvider.MarkUserAsAuthenticated(loginRequest.Email);
            Navigation.NavigateTo("posts");
        }
    }

    private async Task Register(RegisterRequest registerRequest)
    {
        var response = await Client.PostAsJsonAsync("api/auth/register", registerRequest);
        if (response.IsSuccessStatusCode)
        {
            Navigation.NavigateTo("authorize");
        }
    }

    private void ChangeAction() => _loginAction = !_loginAction;
}