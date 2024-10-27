using Microsoft.AspNetCore.Mvc;
using SocialApp.Common.Likes;
using SocialApp.Data.Repositories;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikesController : ControllerBase
{
    private readonly ILikeRepository _likeRepository;
    private readonly ILikeHandler _likeHandler;

    public LikesController(
        ILikeRepository likeRepository,
        ILikeHandler likeHandler)
    {
        _likeRepository = likeRepository;
        _likeHandler = likeHandler;
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetLikesCount([FromQuery] Guid postId)
    {
        return Ok(await _likeRepository.GetLikesCountForPost(postId, HttpContext.RequestAborted));
    }

    [HttpPost("post")]
    public async Task<IActionResult> LikePost([FromQuery] Guid postId, [FromQuery] Guid userId)
    {
        await _likeHandler.LikePost(postId, userId, HttpContext.RequestAborted);
        return NoContent();
    }

    [HttpDelete("post")]
    public async Task<IActionResult> UnlikePost([FromQuery] Guid postId, [FromQuery] Guid userId)
    {
        await _likeHandler.UnlikePost(postId, userId, HttpContext.RequestAborted);
        return NoContent();
    }
}