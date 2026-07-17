using System.Text;
using BiletSatis.Application.Services;
using BiletSatis.Infrastructure.Persistence;
using BiletSatis.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<BiletSatisDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Servisler
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISalonService, SalonService>();
builder.Services.AddScoped<IBolumService, BolumService>();
builder.Services.AddScoped<IEtkinlikService, EtkinlikService>();
builder.Services.AddScoped<IEtkinlikBolumService, EtkinlikBolumService>();
builder.Services.AddScoped<IKoltukService, KoltukService>();

// JWT doğrulama
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();   // önce kimlik doğrulama
app.UseAuthorization();    // sonra yetkilendirme

app.MapControllers();

app.Run();