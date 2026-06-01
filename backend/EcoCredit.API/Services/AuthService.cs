using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using EcoCredit.API.DTOs.Auth;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Services;

public class AuthService {
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepo, IConfiguration config) {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto) {
        var user = await _userRepo.FindByEmailAsync(dto.Email);
        if (user == null) return null;

        bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!valid) return null;

        var token = GenerateJwt(user);
        var expiresAt = DateTime.UtcNow.AddHours(
            _config.GetValue<int>("Jwt:ExpirationHours", 8));

        return new LoginResponseDto {
            Token = token,
            UserId = user.UserId,
            CompanyId = user.CompanyId,
            Role = user.Role,
            ExpiresAt = expiresAt
        };
    }

    public async Task<bool> RegisterAsync(RegisterUserDto dto) {
        var existing = await _userRepo.FindByEmailAsync(dto.Email);
        if (existing != null) return false;

        var user = new Models.User {
            Email = dto.Email,
            CompanyId = dto.CompanyId,
            Role = dto.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12)
        };
        await _userRepo.AddAsync(user);
        return true;
    }

    private string GenerateJwt(Models.User user) {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.UserId),
            new Claim("company_id", user.CompanyId),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer:   _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims:   claims,
            expires:  DateTime.UtcNow.AddHours(_config.GetValue<int>("Jwt:ExpirationHours", 8)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
