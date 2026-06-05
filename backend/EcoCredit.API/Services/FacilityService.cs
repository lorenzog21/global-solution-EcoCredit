using EcoCredit.API.DTOs.Facility;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Services;

public class FacilityService {
    private readonly IFacilityRepository _repo;

    public FacilityService(IFacilityRepository repo) {
        _repo = repo;
    }

    public async Task<IEnumerable<FacilityResponseDto>> GetByCompanyAsync(string companyId) {
        var facilities = await _repo.GetByCompanyAsync(companyId);
        return facilities.Select(ToDto);
    }

    public async Task<FacilityResponseDto?> GetByIdAsync(string id) {
        var facility = await _repo.GetByIdAsync(id);
        return facility == null ? null : ToDto(facility);
    }

    public async Task<FacilityResponseDto> CreateAsync(FacilityCreateDto dto) {
        var facility = new Facility {
            CompanyId         = dto.CompanyId,
            Name              = dto.Name,
            FacilityType      = dto.FacilityType,
            Latitude          = dto.Latitude,
            Longitude         = dto.Longitude,
            EmissionLimitTco2 = dto.EmissionLimitTco2
        };
        await _repo.AddAsync(facility);
        return ToDto(facility);
    }

    public async Task SoftDeleteAsync(string id) =>
        await _repo.SoftDeleteAsync(id);

    private static FacilityResponseDto ToDto(Facility f) => new() {
        FacilityId        = f.FacilityId,
        CompanyId         = f.CompanyId,
        Name              = f.Name,
        FacilityType      = f.FacilityType,
        Latitude          = f.Latitude,
        Longitude         = f.Longitude,
        EmissionLimitTco2 = f.EmissionLimitTco2,
        Active            = f.Active,
        CreatedAt         = f.CreatedAt
    };
}
