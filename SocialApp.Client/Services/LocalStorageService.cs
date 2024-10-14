using Microsoft.JSInterop;

namespace SocialApp.Client.Services;

public interface ILocalStorageService
{
    Task SetItem(string key, string value);
    Task<string> GetItem(string key);
    Task RemoveItem(string key);
    Task Clear();
}

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetItem(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public async Task<string> GetItem(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    public async Task RemoveItem(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task Clear()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }
}