using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using SocialApp.Client.Services;

namespace SocialApp.Client.Authorization;

public class UserAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;

    public UserAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();

        var email = await _localStorage.GetItem("email") ?? string.Empty;

        if (email != string.Empty)
        {
            identity = new([new(ClaimTypes.Email, email)], "apiauth");
            var storedUser = await _httpClient.GetFromJsonAsync<IdentityUser>("/api/Users/?username=" + email);
            identity.AddClaim(new Claim(ClaimTypes.Sid, storedUser.Id));
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    public void MarkUserAsAuthenticated(string email)
    {
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, email)], "apiauth");
        var user = new ClaimsPrincipal(identity);

        _localStorage.SetItem("email", email);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        _localStorage.RemoveItem("email");

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
    }
}