using System.Net.Http.Json;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Services;

public interface IProfileService
{
    Task<ProfileDto> Get(string email);
    Task<ProfileDto> Get();
    Task<ProfileDto> GetByUsername(string username);
    Task Update(ProfileDto profile);
}

public class ProfileService : IProfileService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public ProfileService(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }

    public async Task<ProfileDto> Get()
    {
        var email = await _localStorageService.GetItem("email");
        return await Get(email);
    }

    public async Task<ProfileDto> Get(string email) =>
        await _httpClient.GetFromJsonAsync<ProfileDto>($"/api/profiles?email={email}");

    public async Task<ProfileDto> GetByUsername(string username) =>
        await _httpClient.GetFromJsonAsync<ProfileDto>($"/api/profiles?username={username}");

    public async Task Update(ProfileDto profile) =>
        await _httpClient.PutAsJsonAsync($"/api/profiles", profile);
}