using EcoCredit.API.DTOs.Alert;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Services;

public class AlertService {
    private readonly IAlertRepository _repo;
    private readonly IFacilityRepository _facilityRepo;

    public AlertService(IAlertRepository repo, IFacilityRepository facilityRepo) {
        _repo         = repo;
        _facilityRepo = facilityRepo;
    }

    public async Task<IEnumerable<AlertResponseDto>> GetByCompanyAsync(string companyId, bool activeOnly) {
        var alerts = await _repo.GetByCompanyAsync(companyId, activeOnly);
        var result = new List<AlertResponseDto>();

        foreach (var a in alerts) {
            var facilityName = a.Facility?.Name ?? string.Empty;
            if (string.IsNullOrEmpty(facilityName)) {
                var f = await _facilityRepo.GetByIdAsync(a.FacilityId);
                facilityName = f?.Name ?? a.FacilityId;
            }
            result.Add(new AlertResponseDto {
                AlertId      = a.AlertId,
                FacilityId   = a.FacilityId,
                FacilityName = facilityName,
                AlertType    = a.AlertType,
                Severity     = a.Severity,
                Message      = a.Message,
                Resolved     = a.Resolved,
                ResolvedAt   = a.ResolvedAt,
                CreatedAt    = a.CreatedAt
            });
        }
        return result;
    }

    public async Task ResolveAsync(string alertId, string note) =>
        await _repo.ResolveAsync(alertId, note);
}
