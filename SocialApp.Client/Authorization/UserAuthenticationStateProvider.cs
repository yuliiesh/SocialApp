using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Xml.Schema;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualBasic;
using SocialApp.Client.Services;

namespace SocialApp.Client.Authorization;

public class UserAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public UserAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();

        var email = await _localStorage.GetItem("email") ?? string.Empty;

        if (email != string.Empty)
        {
            identity = new([new(ClaimTypes.Email, email)], "apiauth");
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