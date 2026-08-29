namespace Api.Modules.Auth.Models;

// This is what we send BACK to the frontend after a successful login
// The Token is a JWT token the frontend stores and sends with every request
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}