using Microsoft.AspNetCore.Mvc;
using SocialApp.Common.Friends;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FriendsController : ControllerBase
{
    private readonly IFriendHandler _friendHandler;

    public FriendsController(IFriendHandler friendHandler)
    {
        _friendHandler = friendHandler;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetFriends([FromRoute] Guid userId)
    {
        var friends = await _friendHandler.GetFriendsInfo(userId, HttpContext.RequestAborted);

        return Ok(friends);
    }

    [HttpPatch("{userId:guid}")]
    public async Task<IActionResult> AddFriend([FromRoute] Guid userId, [FromBody] Guid friendId)
    {
        await _friendHandler.AddFriend(userId, friendId, HttpContext.RequestAborted);
        return NoContent();
    }

    [HttpPatch("{userId:guid}/unfriend")]
    public async Task<IActionResult> RemoveFriend([FromRoute] Guid userId, [FromBody] Guid friendId)
    {
        await _friendHandler.RemoveFriend(userId, friendId, HttpContext.RequestAborted);
        return NoContent();
    }
}