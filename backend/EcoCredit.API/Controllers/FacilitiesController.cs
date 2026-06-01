using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/facilities")]
[Authorize]
public class FacilitiesController : ControllerBase {
    private readonly IFacilityRepository _repo;

    public FacilitiesController(IFacilityRepository repo) {
        _repo = repo;
    }

    /// <summary>Lista instalações da empresa autenticada</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var companyId = User.FindFirst("company_id")?.Value ?? string.Empty;
        var facilities = await _repo.GetByCompanyAsync(companyId);
        return Ok(facilities);
    }

    /// <summary>Cadastra nova instalação</summary>
    [HttpPost]
    [Authorize(Roles = "ADMIN,ANALYST")]
    public async Task<IActionResult> Create([FromBody] Facility facility) {
        facility.CompanyId = User.FindFirst("company_id")?.Value ?? string.Empty;
        await _repo.AddAsync(facility);
        return CreatedAtAction(nameof(GetAll), new { }, facility);
    }

    /// <summary>Atualiza dados de uma instalação</summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,ANALYST")]
    public async Task<IActionResult> Update(string id, [FromBody] Facility facility) {
        facility.FacilityId = id;
        await _repo.UpdateAsync(facility);
        return NoContent();
    }

    /// <summary>Desativa instalação (soft delete)</summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(string id) {
        await _repo.SoftDeleteAsync(id);
        return NoContent();
    }
}
