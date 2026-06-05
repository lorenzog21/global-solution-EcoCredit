using EcoCredit.API.DTOs.Credit;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Services;

public class CreditService {
    private readonly ICreditRepository _repo;

    public CreditService(ICreditRepository repo) {
        _repo = repo;
    }

    public async Task<CreditBalanceDto> GetBalanceAsync(string companyId) {
        var credits = (await _repo.GetByCompanyAsync(companyId)).ToList();
        return new CreditBalanceDto {
            TotalAvailableTco2 = credits.Where(c => c.Status == "AVAILABLE").Sum(c => c.QuantityTco2),
            TotalUsedTco2      = credits.Where(c => c.Status == "USED").Sum(c => c.QuantityTco2),
            AvailableCount     = credits.Count(c => c.Status == "AVAILABLE"),
            Credits            = credits.Select(ToDto)
        };
    }

    public async Task<CreditResponseDto> CreateAsync(string companyId, CreditCreateDto dto) {
        var credit = new CarbonCredit {
            CompanyId    = companyId,
            QuantityTco2 = dto.QuantityTco2,
            CreditType   = dto.CreditType,
            Status       = "AVAILABLE",
            PriceUsd     = dto.PriceUsd,
            ExpiryDate   = dto.ExpiryDate,
            RegistryCode = dto.RegistryCode
        };
        await _repo.AddAsync(credit);
        return ToDto(credit);
    }

    public async Task<CreditResponseDto?> UseCreditAsync(string creditId) {
        var credit = await _repo.GetByIdAsync(creditId);
        if (credit == null || credit.Status != "AVAILABLE") return null;

        credit.Status = "USED";
        await _repo.UpdateAsync(credit);
        return ToDto(credit);
    }

    private static CreditResponseDto ToDto(CarbonCredit c) => new() {
        CreditId     = c.CreditId,
        CompanyId    = c.CompanyId,
        QuantityTco2 = c.QuantityTco2,
        CreditType   = c.CreditType,
        Status       = c.Status,
        PriceUsd     = c.PriceUsd,
        IssuedDate   = c.IssuedDate,
        ExpiryDate   = c.ExpiryDate,
        RegistryCode = c.RegistryCode
    };
}
