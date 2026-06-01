using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public interface ICreditRepository {
    Task<IEnumerable<CarbonCredit>> GetByCompanyAsync(string companyId);
    Task<CarbonCredit?> GetByIdAsync(string id);
    Task AddAsync(CarbonCredit credit);
    Task UpdateAsync(CarbonCredit credit);
}
