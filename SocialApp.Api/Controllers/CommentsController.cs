using Microsoft.AspNetCore.Mvc;
using SocialApp.Common.Comments;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentHandler _commentHandler;

    public CommentsController(ICommentHandler commentHandler)
    {
        _commentHandler = commentHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid postId)
    {
        var comments = await _commentHandler.GetCommentsByPostId(postId, HttpContext.RequestAborted);
        return Ok(comments);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCommentsCount([FromQuery] Guid postId)
    {
        var count = await _commentHandler.GetCommentsCount(postId, HttpContext.RequestAborted);
        return Ok(count);
    }

    [HttpPost("post/{postId:guid}")]
    public async Task<IActionResult> CreateComment([FromRoute] Guid postId, [FromBody] CommentDto comment)
    {
        await _commentHandler.CreateComment(postId, comment, HttpContext.RequestAborted);
        return Ok();
    }
}