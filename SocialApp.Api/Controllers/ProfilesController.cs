using Microsoft.AspNetCore.Mvc;
using SocialApp.Common.Profiles;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IProfileHandler _profileHandler;

    public ProfilesController(IProfileHandler profileHandler)
    {
        _profileHandler = profileHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile([FromQuery] string email, [FromQuery] string username)
    {
        ProfileDto profileDto;
        if (!string.IsNullOrEmpty(username))
        {
            profileDto = await _profileHandler.GetByUsername(username, HttpContext.RequestAborted);
        }
        else
        {
            profileDto = await _profileHandler.Get(email, HttpContext.RequestAborted);
        }

        return Ok(profileDto);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] ProfileDto profileDto)
    {
        await _profileHandler.Update(profileDto, HttpContext.RequestAborted);
        return Ok();
    }
}