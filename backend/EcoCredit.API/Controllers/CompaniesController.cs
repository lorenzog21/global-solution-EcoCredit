using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/companies")]
[Authorize]
public class CompaniesController : ControllerBase {
    private readonly ICompanyRepository _repo;

    public CompaniesController(ICompanyRepository repo) {
        _repo = repo;
    }

    /// <summary>Lista todas as empresas cadastradas</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var companies = await _repo.GetAllAsync();
        return Ok(companies);
    }

    /// <summary>Busca empresa por ID</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id) {
        var company = await _repo.GetByIdAsync(id);
        if (company == null) return NotFound();
        return Ok(company);
    }

    /// <summary>Cadastra nova empresa</summary>
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Create([FromBody] Company company) {
        await _repo.AddAsync(company);
        return CreatedAtAction(nameof(GetById), new { id = company.CompanyId }, company);
    }
}
