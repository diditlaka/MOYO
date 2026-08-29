using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Api.Infrastructure.Persistence;
using Api.Modules.Auth;
using Api.Modules.Products;
using Api.Modules.Orders;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────
// SERVICES
// ─────────────────────────────────────────────

// Controllers — this tells the app we have API controllers
builder.Services.AddControllers();

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

// CORS — allows our Angular front end to talk to this API
// Without this the browser would block the requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Our own services — this registers our business logic classes
// AddScoped means a new instance is created per HTTP request
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// ─────────────────────────────────────────────
// MIDDLEWARE PIPELINE
// ─────────────────────────────────────────────

var app = builder.Build();

// The order here matters — each request flows through these in order
app.UseCors("AllowAngular");        // 1. Allow Angular requests
app.UseAuthentication();             // 2. Check who the user is
app.UseAuthorization();              // 3. Check what they're allowed to do
app.MapControllers();                // 4. Route to the right controller

app.Run();