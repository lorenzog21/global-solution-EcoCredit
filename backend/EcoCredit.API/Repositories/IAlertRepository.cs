using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public interface IAlertRepository {
    Task<IEnumerable<Alert>> GetByCompanyAsync(string companyId, bool activeOnly);
    Task AddAsync(Alert alert);
    Task ResolveAsync(string alertId, string note);
}
