using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ResearchAgent.Data;
using ResearchAgent.DTOs;
using ResearchAgent.Models;

namespace ResearchAgent.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config) { _db = db; _config = config; }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        if (_db.Users.Any(u => u.Email == dto.Email)) return null;
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return new AuthResponseDto(GenerateToken(user), user.Name, user.Email);
    }

    public AuthResponseDto? Login(LoginDto dto)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return null;
        return new AuthResponseDto(GenerateToken(user), user.Name, user.Email);
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt__Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(30), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
