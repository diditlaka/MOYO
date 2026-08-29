namespace Api.Modules.Auth.Models;

// This is what the frontend sends when a user tries to log in
// Just their email and password — nothing else needed
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}