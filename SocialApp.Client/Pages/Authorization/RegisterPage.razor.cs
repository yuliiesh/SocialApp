using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using SocialApp.Common.Authorization;

namespace SocialApp.Client.Pages.Authorization;

public partial class RegisterPage : ComponentBase
{
    [Inject] private HttpClient Client { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }

    private RegisterRequest registerRequest = new();

    private async Task HandleRegister()
    {
        var response = await Client.PostAsJsonAsync("api/auth/register", registerRequest);
        if (response.IsSuccessStatusCode)
        {
            Navigation.NavigateTo("/login");
        }
    }
}