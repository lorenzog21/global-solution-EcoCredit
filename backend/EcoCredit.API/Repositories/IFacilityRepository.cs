using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public interface IFacilityRepository {
    Task<IEnumerable<Facility>> GetByCompanyAsync(string companyId);
    Task<Facility?> GetByIdAsync(string id);
    Task AddAsync(Facility facility);
    Task UpdateAsync(Facility facility);
    Task SoftDeleteAsync(string id);
}
