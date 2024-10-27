using System.Net.Http.Json;
using SocialApp.Common.Friends;

namespace SocialApp.Client.Services;

public interface IFriendService
{
    Task AddFriend(Guid userId, Guid friendId);
    Task<IReadOnlyList<FriendDto>> GetFriends(Guid userId);

    Task RemoveFriend(Guid userId, Guid friendId);
}

public class FriendService : IFriendService
{
    private readonly HttpClient _httpClient;

    public FriendService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task AddFriend(Guid userId, Guid friendId) =>
        await _httpClient.PatchAsJsonAsync($"api/friends/{userId}", friendId);

    public async Task<IReadOnlyList<FriendDto>> GetFriends(Guid userId) =>
        await _httpClient.GetFromJsonAsync<IReadOnlyList<FriendDto>>($"api/friends/{userId}");

    public async Task RemoveFriend(Guid userId, Guid friendId) =>
        await _httpClient.PatchAsJsonAsync($"api/friends/{userId}/unfriend", friendId);
}