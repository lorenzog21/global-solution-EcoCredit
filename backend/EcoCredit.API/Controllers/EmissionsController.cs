using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.DTOs.Emission;
using EcoCredit.API.Services;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/emissions")]
[Authorize]
public class EmissionsController : ControllerBase {
    private readonly EmissionService _service;

    public EmissionsController(EmissionService service) {
        _service = service;
    }

    /// <summary>Registra nova emissão (manual ou via payload do simulador IoT)</summary>
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] EmissionCreateDto dto) {
        if (dto.QuantityTco2e <= 0)
            return BadRequest(new { error = "Quantidade deve ser positiva." });

        var record = await _service.RegisterAsync(dto);
        return CreatedAtAction(nameof(GetSummary), new { }, record);
    }

    /// <summary>Sumário de emissões por instalação da empresa autenticada</summary>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary() {
        var companyId = User.FindFirst("company_id")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        var summary = await _service.GetSummaryAsync(companyId);
        return Ok(summary);
    }
}
