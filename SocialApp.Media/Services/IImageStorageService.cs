using SocialApp.Common.Images;

namespace SocialApp.Media.Services;

public interface IImageStorageService
{
    Task<ImageInfo> UploadImage(string id, IFormFile file);
    Task<ImageInfo> GetImageInfo(string id, string fileName);
    Task<byte[]> GetImage(string id, string fileName);
    Task<bool> DeleteImage(string id, string fileName);

    Task<ICollection<ImageInfo>> UploadImages(string id, List<IFormFile> files);
}