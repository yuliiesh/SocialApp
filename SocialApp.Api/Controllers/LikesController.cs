using Microsoft.AspNetCore.Mvc;
using SocialApp.Data.Repositories;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikesController : ControllerBase
{
    private readonly ILikeRepository _likeRepository;

    public LikesController(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetLikesCount([FromQuery] Guid postId)
    {
        return Ok(await _likeRepository.GetLikesCountForPost(postId, HttpContext.RequestAborted));
    }
}