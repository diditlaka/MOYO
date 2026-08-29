namespace Api.Controllers;

using Api.Modules.Auth;
using Api.Modules.Auth.Models;
using Microsoft.AspNetCore.Mvc;

// [ApiController] tells .NET this is an API controller
// [Route] sets the URL prefix — so all auth endpoints start with /api/auth
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    // IAuthService is injected automatically by .NET
    // We never create it ourselves — .NET handles that
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST /api/auth/register
    // Called when a new user creates an account
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        // If result is null it means the email is already taken
        if (result == null)
            return BadRequest(new { message = "Email already in use" });

        // 201 Created — standard response for successfully creating a resource
        return CreatedAtAction(nameof(Register), result);
    }

    // POST /api/auth/login
    // Called when an existing user logs in
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        // If result is null the email or password was wrong
        if (result == null)
            return Unauthorized(new { message = "Invalid email or password" });

        // 200 OK — return the token and user info
        return Ok(result);
    }
}