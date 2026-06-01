using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.DTOs.Auth;
using EcoCredit.API.Services;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase {
    private readonly AuthService _authService;

    public AuthController(AuthService authService) {
        _authService = authService;
    }

    /// <summary>Login — retorna JWT para uso nos demais endpoints</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto) {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { error = "Email e senha são obrigatórios." });

        if (!dto.Email.Contains('@') || dto.Email.Length > 200)
            return BadRequest(new { error = "Email inválido." });

        var result = await _authService.LoginAsync(dto);
        if (result == null)
            return Unauthorized(new { error = "Credenciais inválidas." });

        return Ok(result);
    }

    /// <summary>Registra novo usuário (apenas ADMIN pode registrar)</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto) {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { error = "Email e senha são obrigatórios." });

        if (dto.Password.Length < 8)
            return BadRequest(new { error = "Senha deve ter ao menos 8 caracteres." });

        var success = await _authService.RegisterAsync(dto);
        if (!success)
            return Conflict(new { error = "Email já cadastrado." });

        return StatusCode(201, new { message = "Usuário criado com sucesso." });
    }
}
