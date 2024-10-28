using System.Net;
using SocialApp.Common.Images;

namespace SocialApp.Media.Services;

public class ImageStorageService : IImageStorageService
{
    private readonly string _imageFolder;

    private readonly ILogger<ImageStorageService> _logger;

    public ImageStorageService(ILogger<ImageStorageService> logger)
    {
        _logger = logger;
        _imageFolder = Path.Combine("images");

        if (!Directory.Exists(_imageFolder))
        {
            Directory.CreateDirectory(_imageFolder);
        }
    }

    public async Task<ImageInfo> UploadImage(string id, IFormFile file) =>
        (await UploadImages(id, [file])).FirstOrDefault();

    public Task<ImageInfo> GetImageInfo(string id, string fileName)
    {
        var filePath = Path.Combine(_imageFolder, id, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Image not found");
        }

        var fileInfo = new FileInfo(filePath);

        var imageInfo = new ImageInfo
        {
            FileName = fileName,
            StoredFileName = filePath,
            FileSize = fileInfo.Length,
            UploadDate = fileInfo.CreationTimeUtc
        };

        return Task.FromResult(imageInfo);
    }

    public async Task<byte[]> GetImage(string id, string fileName)
    {
        var filePath = Path.Combine(_imageFolder, id, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Image not found");
        }

        var imageBytes = await File.ReadAllBytesAsync(filePath);

        return imageBytes;
    }

    public Task<bool> DeleteImage(string id, string fileName)
    {
        var filePath = Path.Combine(_imageFolder, id, fileName);

        if (!File.Exists(filePath))
        {
            return Task.FromResult(false);
        }

        File.Delete(filePath);

        return Task.FromResult(true);
    }

    public async Task<ICollection<ImageInfo>> UploadImages(string id, List<IFormFile> files)
    {
        var directoryPath = Path.Combine(_imageFolder, id);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var result = new List<ImageInfo>(files.Count);

        foreach (var file in files)
        {
            var uploadResult = new ImageInfo();
            var untrustedFileName = file.FileName;
            uploadResult.FileName = file.FileName;
            var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

            if (file.Length == 0)
            {
                _logger.LogInformation("{FileName} length is 0 (Err: 1)", trustedFileNameForDisplay);
            }
            else
            {
                try
                {
                    var trustedFileNameForFileStorage = string.Concat(Path.ChangeExtension(Path.GetRandomFileName(), string.Empty), Path.GetExtension(file.FileName));

                    var path = Path.Combine(directoryPath, trustedFileNameForFileStorage);

                    await using FileStream fs = new(path, FileMode.Create);
                    await file.CopyToAsync(fs);

                    _logger.LogInformation("{FileName} saved at {Path}", trustedFileNameForDisplay, path);
                    uploadResult.StoredFileName = trustedFileNameForFileStorage;
                }
                catch (IOException ex)
                {
                    _logger.LogError("{FileName} error on upload (Err: 3): {Message}", trustedFileNameForDisplay, ex.Message);
                }
            }

            result.Add(uploadResult);
        }

        return result;
    }
}