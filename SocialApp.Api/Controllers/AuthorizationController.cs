using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Common.Profiles;
using SocialApp.Common.Profiles.Create;
using LoginRequest = SocialApp.Common.Authorization.LoginRequest;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IProfileHandler _profileHandler;

    public AuthorizationController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IProfileHandler profileHandler)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _profileHandler = profileHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] SocialApp.Common.Authorization.RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _profileHandler.CreateProfile(new CreateProfileRequest
            {
                FirstName = request.FirstName,
                Email = request.Email,
                LastName = request.LastName,
                Username = request.Username,
                UserId = Guid.Parse(user.Id)
            },
            HttpContext.RequestAborted);

        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (result.Succeeded)
        {
            return Ok(new { Message = "Logged in successfully" });
        }

        return Unauthorized();
    }
}