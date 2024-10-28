using Microsoft.AspNetCore.Mvc;
using SocialApp.Media.Services;

namespace SocialApp.Media.Controllers;

 [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageStorageService _imageStorageService;

        public ImageController(IImageStorageService imageStorageService)
        {
            _imageStorageService = imageStorageService;
        }

        [HttpPost("upload/{id}")]
        public async Task<IActionResult> UploadFile([FromRoute] string id, IFormFile mainPhoto, List<IFormFile> files)
        {
            if (mainPhoto is not null)
            {
                files ??= [];
                files.Insert(0, mainPhoto);
            }

            if (files is null || !files.Any())
            {
                return BadRequest("No file uploaded");
            }

            var filePaths = await _imageStorageService.UploadImages(id, files);
            return Ok(filePaths);
        }

        [HttpGet("{id}/{fileName}")]
        public async Task<IActionResult> GetImage([FromRoute] string id, [FromRoute] string fileName)
        {
            try
            {
                var imageBytes = await _imageStorageService.GetImage(id, fileName);
                return File(imageBytes, "application/octet-stream", fileName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("info/{id}/{fileName}")]
        public async Task<IActionResult> GetImageInfo([FromRoute] string id, [FromRoute] string fileName)
        {
            try
            {
                var imageInfo = await _imageStorageService.GetImageInfo(id, fileName);
                return Ok(imageInfo);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{id}/{fileName}")]
        public async Task<IActionResult> DeleteImage([FromRoute] string id, [FromRoute] string fileName)
        {
            try
            {
                var result = await _imageStorageService.DeleteImage(id, fileName);
                if (result)
                {
                    return Ok();
                }

                return NotFound("Image not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }