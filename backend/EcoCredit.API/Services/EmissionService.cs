using EcoCredit.API.DTOs.Emission;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Services;

public class EmissionService {
    private readonly IEmissionRepository _emissionRepo;
    private readonly IFacilityRepository _facilityRepo;
    private readonly IAlertRepository _alertRepo;

    public EmissionService(
        IEmissionRepository emissionRepo,
        IFacilityRepository facilityRepo,
        IAlertRepository alertRepo) {
        _emissionRepo = emissionRepo;
        _facilityRepo = facilityRepo;
        _alertRepo = alertRepo;
    }

    public async Task<EmissionRecord> RegisterAsync(EmissionCreateDto dto) {
        var record = new EmissionRecord {
            FacilityId           = dto.FacilityId,
            GasType              = dto.GasType,
            QuantityTco2e        = dto.QuantityTco2e,
            Source               = dto.Source,
            SensorId             = dto.SensorId,
            RawPpm               = dto.RawPpm,
            SatelliteCo2Regional = dto.SatelliteCo2Regional
        };
        await _emissionRepo.AddAsync(record);
        await CheckThresholdAsync(dto.FacilityId);
        return record;
    }

    public async Task<IEnumerable<EmissionSummaryDto>> GetSummaryAsync(string companyId) {
        var facilities = await _facilityRepo.GetByCompanyAsync(companyId);
        var summaries = new List<EmissionSummaryDto>();

        foreach (var f in facilities) {
            var monthlyTotal = await _emissionRepo.GetMonthlyTotalAsync(f.FacilityId);
            var pct = f.EmissionLimitTco2 > 0
                ? Math.Round((monthlyTotal / f.EmissionLimitTco2) * 100, 1)
                : 0;

            summaries.Add(new EmissionSummaryDto {
                FacilityId        = f.FacilityId,
                FacilityName      = f.Name,
                TotalEmittedTco2e = monthlyTotal,
                EmissionLimitTco2 = f.EmissionLimitTco2,
                PercentUsed       = pct,
                ComplianceStatus  = pct >= 100 ? "CRITICAL"
                                  : pct >= 80  ? "WARNING"
                                               : "NORMAL"
            });
        }
        return summaries;
    }

    private async Task CheckThresholdAsync(string facilityId) {
        var facility = await _facilityRepo.GetByIdAsync(facilityId);
        if (facility == null) return;

        var monthlyTotal = await _emissionRepo.GetMonthlyTotalAsync(facilityId);
        var pct = facility.EmissionLimitTco2 > 0
            ? monthlyTotal / facility.EmissionLimitTco2
            : 0;

        if (pct >= 1.0m) {
            await _alertRepo.AddAsync(new Alert {
                FacilityId = facilityId,
                AlertType  = "THRESHOLD",
                Severity   = "CRITICAL",
                Message    = $"Instalação {facility.Name} ultrapassou 100% do limite mensal ({monthlyTotal:F2} tCO2e / {facility.EmissionLimitTco2} tCO2e)."
            });
        } else if (pct >= 0.8m) {
            await _alertRepo.AddAsync(new Alert {
                FacilityId = facilityId,
                AlertType  = "THRESHOLD",
                Severity   = "HIGH",
                Message    = $"Instalação {facility.Name} atingiu {pct:P0} do limite mensal."
            });
        }
    }
}
