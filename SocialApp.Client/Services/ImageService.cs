using System.Net.Http.Json;
using SocialApp.Common.Images;

namespace SocialApp.Client.Services;

public interface IImageService
{
    Task<IReadOnlyCollection<ImageInfo>> Upload(Guid id, MultipartFormDataContent content, CancellationToken cancellationToken);
    string GetImageUrl(Guid id, string fileName);
}

public class ImageService : IImageService
{
    private readonly HttpClient _httpClient;

    public ImageService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("ApiSettings:StorageUrl"));
    }

    public async Task<IReadOnlyCollection<ImageInfo>> Upload(Guid id, MultipartFormDataContent content, CancellationToken cancellationToken)
    {
        var response =  await _httpClient.PostAsync($"/api/Image/upload/{id}", content, cancellationToken);

        return await response.Content.ReadFromJsonAsync<IReadOnlyCollection<ImageInfo>>(cancellationToken);
    }

    public string GetImageUrl(Guid id, string fileName) =>
        string.Concat(_httpClient.BaseAddress!.AbsoluteUri, $"api/Image/{id}/{fileName}");
}