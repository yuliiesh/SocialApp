using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Comments;
using SocialApp.Data.Repositories;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid postId)
    {
        var comments = await _commentRepository.GetCommentsByPostId(postId, HttpContext.RequestAborted);
        return Ok(comments.Select(x => x.ToDto()));
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCommentsCount([FromQuery] Guid postId)
    {
        var count = await _commentRepository.GetCommentsCount(postId, HttpContext.RequestAborted);
        return Ok(count);
    }
}