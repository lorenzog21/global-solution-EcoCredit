using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public interface ICompanyRepository {
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(string id);
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
}
