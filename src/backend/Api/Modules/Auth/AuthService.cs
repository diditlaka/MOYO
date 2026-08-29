namespace Api.Modules.Auth;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Infrastructure.Persistence;
using Api.Modules.Auth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// AuthService contains the actual business logic for login and register
// It talks to the database through AppDbContext
public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    // The constructor receives AppDbContext and IConfiguration
    // through Dependency Injection — .NET handles creating these for us
    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        // Check if a client with this email already exists
        var exists = await _db.Clients.AnyAsync(c => c.Email == request.Email);
        if (exists) return null; // Return null to signal the email is taken

        // Hash the password using BCrypt before storing it
        // We NEVER store plain text passwords
        var client = new Client
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        // Save the new client to the database
        _db.Clients.Add(client);
        await _db.SaveChangesAsync();

        // Generate a JWT token so they're logged in immediately after registering
        return new AuthResponse
        {
            Token = GenerateToken(client),
            Name = client.Name,
            Email = client.Email
        };
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        // Find the client by email
        var client = await _db.Clients
            .FirstOrDefaultAsync(c => c.Email == request.Email);

        // If client doesn't exist or password is wrong, return null
        // BCrypt.Verify compares the plain password against the stored hash
        if (client == null || !BCrypt.Net.BCrypt.Verify(request.Password, client.PasswordHash))
            return null;

        // Password is correct — generate and return a JWT token
        return new AuthResponse
        {
            Token = GenerateToken(client),
            Name = client.Name,
            Email = client.Email
        };
    }

    // GenerateToken creates a JWT token for the client
    // A JWT token is like a digital ID card — it contains the client's info
    // and is signed with our secret key so we know it's genuine
    private string GenerateToken(Client client)
    {
        var claims = new[]
        {
            // Claims are pieces of info we embed in the token
            new Claim(ClaimTypes.NameIdentifier, client.ClientId.ToString()),
            new Claim(ClaimTypes.Email, client.Email),
            new Claim(ClaimTypes.Name, client.Name)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8), // Token expires after 8 hours
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}