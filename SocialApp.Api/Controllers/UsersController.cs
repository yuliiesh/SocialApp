using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SocialApp.Api.Controllers;

[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public UsersController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        return Ok(user);
    }
}