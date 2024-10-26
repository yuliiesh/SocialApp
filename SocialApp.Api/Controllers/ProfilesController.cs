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
    public async Task<IActionResult> GetProfile([FromQuery] string email)
    {
        var profileDto = await _profileHandler.Get(email, HttpContext.RequestAborted);

        return Ok(profileDto);
    }
}