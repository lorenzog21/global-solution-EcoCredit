using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/alerts")]
[Authorize]
public class AlertsController : ControllerBase {
    private readonly IAlertRepository _repo;

    public AlertsController(IAlertRepository repo) {
        _repo = repo;
    }

    /// <summary>Lista alertas ativos e histórico da empresa</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = true) {
        var companyId = User.FindFirst("company_id")?.Value ?? string.Empty;
        var alerts = await _repo.GetByCompanyAsync(companyId, activeOnly);
        return Ok(alerts);
    }

    /// <summary>Marca alerta como resolvido</summary>
    [HttpPut("{id}/resolve")]
    public async Task<IActionResult> Resolve(string id, [FromBody] ResolveAlertDto dto) {
        await _repo.ResolveAsync(id, dto.Note);
        return NoContent();
    }
}

public record ResolveAlertDto(string Note);
