namespace Api.Modules.Auth;

using Api.Modules.Auth.Models;

// An interface defines WHAT a service can do
// without specifying HOW it does it
// This is a pattern called Dependency Injection
// — Program.cs registers AuthService as the implementation
// — Controllers just ask for IAuthService and don't care which version they get
public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}