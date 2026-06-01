using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public interface IEmissionRepository {
    Task<IEnumerable<EmissionRecord>> GetByFacilityAsync(string facilityId);
    Task<decimal> GetMonthlyTotalAsync(string facilityId);
    Task AddAsync(EmissionRecord record);
}
