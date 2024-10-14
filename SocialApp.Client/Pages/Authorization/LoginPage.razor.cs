using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using SocialApp.Client.Authorization;
using SocialApp.Common.Authorization;

namespace SocialApp.Client.Pages.Authorization;

public partial class LoginPage : ComponentBase
{
    [Inject] private HttpClient Client { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private UserAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private LoginRequest loginRequest = new();

    private async Task HandleLogin()
    {
        var response = await Client.PostAsJsonAsync("api/auth/login", loginRequest);
        if (response.IsSuccessStatusCode)
        {
            AuthenticationStateProvider.MarkUserAsAuthenticated(loginRequest.Email);
            Navigation.NavigateTo("/");
        }
    }
}