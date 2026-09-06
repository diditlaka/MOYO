using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Api.Infrastructure.Persistence;
using Api.Modules.Auth;
using Api.Modules.Products;
using Api.Modules.Orders;

var builder = WebApplication.CreateBuilder(args);

// Controllers — this tells the app we have API controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This tells the JSON serializer to handle circular references
        // instead of going into an infinite loop
        options.JsonSerializerOptions.ReferenceHandler = 
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Database — connect to SQL Server using the connection string in appsettings.json
// AppDbContext is our bridge between C# and the database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication — this tells the app how to validate login tokens
// When a client logs in, we give them a token
// They send that token with every request to prove who they are
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Note: CORS is no longer needed — the Angular app is now served
// from this same ASP.NET Core app (same origin), so the browser
// never blocks requests between frontend and backend.

// Our own services — this registers our business logic classes
// AddScoped means a new instance is created per HTTP request
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// The order here matters — each request flows through these in order
app.UseDefaultFiles();               // 1. Serve index.html for / requests
app.UseStaticFiles();                // 2. Serve the built Angular JS/CSS/assets
app.UseAuthentication();             // 3. Check who the user is
app.UseAuthorization();              // 4. Check what they're allowed to do
app.MapControllers();                // 5. Route API calls to the right controller
app.MapFallbackToFile("index.html"); // 6. Any non-API route → let Angular's router handle it

app.Run();