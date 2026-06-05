using EcoCredit.API.DTOs.Company;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Services;

public class CompanyService {
    private readonly ICompanyRepository _repo;

    public CompanyService(ICompanyRepository repo) {
        _repo = repo;
    }

    public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync() {
        var companies = await _repo.GetAllAsync();
        return companies.Select(ToDto);
    }

    public async Task<CompanyResponseDto?> GetByIdAsync(string id) {
        var company = await _repo.GetByIdAsync(id);
        return company == null ? null : ToDto(company);
    }

    public async Task<CompanyResponseDto> CreateAsync(CompanyCreateDto dto) {
        var company = new Company {
            Name    = dto.Name,
            Cnpj    = dto.Cnpj,
            Sector  = dto.Sector,
            Country = dto.Country
        };
        await _repo.AddAsync(company);
        return ToDto(company);
    }

    private static CompanyResponseDto ToDto(Company c) => new() {
        CompanyId = c.CompanyId,
        Name      = c.Name,
        Cnpj      = c.Cnpj,
        Sector    = c.Sector,
        Country   = c.Country,
        Active    = c.Active,
        CreatedAt = c.CreatedAt
    };
}
