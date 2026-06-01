using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/credits")]
[Authorize]
public class CreditsController : ControllerBase {
    private readonly ICreditRepository _repo;

    public CreditsController(ICreditRepository repo) {
        _repo = repo;
    }

    /// <summary>Lista créditos de carbono da empresa com saldo calculado</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var companyId = User.FindFirst("company_id")?.Value ?? string.Empty;
        var credits = await _repo.GetByCompanyAsync(companyId);
        var available = credits.Where(c => c.Status == "AVAILABLE").Sum(c => c.QuantityTco2);
        return Ok(new { credits, totalAvailableTco2 = available });
    }

    /// <summary>Registra aquisição de novos créditos de carbono</summary>
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Add([FromBody] CarbonCredit credit) {
        credit.CompanyId = User.FindFirst("company_id")?.Value ?? string.Empty;
        credit.Status = "AVAILABLE";
        await _repo.AddAsync(credit);
        return CreatedAtAction(nameof(GetAll), new { }, credit);
    }

    /// <summary>Aplica crédito para compensar emissões</summary>
    [HttpPut("{id}/use")]
    [Authorize(Roles = "ADMIN,ANALYST")]
    public async Task<IActionResult> UseCredit(string id) {
        var credit = await _repo.GetByIdAsync(id);
        if (credit == null) return NotFound();
        if (credit.Status != "AVAILABLE")
            return BadRequest(new { error = "Crédito não está disponível." });

        credit.Status = "USED";
        await _repo.UpdateAsync(credit);
        return Ok(credit);
    }
}
