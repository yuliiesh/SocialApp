using System.Net.Http.Json;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Services;

public interface IProfileService
{
    Task<ProfileDto> Get(string email);
    Task<ProfileDto> Get();
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
}