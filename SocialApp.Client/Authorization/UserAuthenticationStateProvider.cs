using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.IO;
using SocialApp.Client.Services;

namespace SocialApp.Client.Authorization;

public class UserAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private readonly IProfileService _profileService;

    public UserAuthenticationStateProvider(
        ILocalStorageService localStorage,
        HttpClient httpClient,
        IProfileService profileService)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
        _profileService = profileService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();

        var email = await _localStorage.GetItem("email") ?? string.Empty;

        if (email != string.Empty)
        {
            identity = new([new(ClaimTypes.Email, email)], "apiauth");

            var profile = await _profileService.Get(email);

            await _localStorage.SetItem("profile", JsonSerializer.Serialize(profile));
            await _localStorage.SetItem("userId", profile.UserId.ToString());

            identity.AddClaim(new Claim(ClaimTypes.Sid, profile.UserId.ToString()));
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    public async Task MarkUserAsAuthenticated(string email)
    {
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, email)], "apiauth");
        var user = new ClaimsPrincipal(identity);

        await _localStorage.SetItem("email", email);

        var profile = await _profileService.Get(email);
        await _localStorage.SetItem("profile", JsonSerializer.Serialize(profile));
        await _localStorage.SetItem("userId", profile.UserId.ToString());

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        _localStorage.RemoveItem("email");
        _localStorage.RemoveItem("profile");
        _localStorage.RemoveItem("userId");

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
    }
}