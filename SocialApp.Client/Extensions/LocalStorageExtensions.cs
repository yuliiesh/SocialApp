using SocialApp.Client.Services;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Extensions;

public static class LocalStorageExtensions
{
    public static async Task<ProfileDto> GetProfile(this ILocalStorageService localStorage) =>
        await localStorage.GetItem<ProfileDto>("profile");

    public static async Task<Guid> GetUserId(this ILocalStorageService localStorage) =>
        Guid.Parse(await localStorage.GetItem("userId"));
}