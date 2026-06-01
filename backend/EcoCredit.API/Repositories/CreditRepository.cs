using Microsoft.EntityFrameworkCore;
using EcoCredit.API.Data;
using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public class CreditRepository : ICreditRepository {
    private readonly AppDbContext _ctx;
    public CreditRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<CarbonCredit>> GetByCompanyAsync(string companyId) =>
        await _ctx.CarbonCredits.Where(c => c.CompanyId == companyId).ToListAsync();

    public async Task<CarbonCredit?> GetByIdAsync(string id) =>
        await _ctx.CarbonCredits.FindAsync(id);

    public async Task AddAsync(CarbonCredit credit) {
        _ctx.CarbonCredits.Add(credit);
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(CarbonCredit credit) {
        _ctx.CarbonCredits.Update(credit);
        await _ctx.SaveChangesAsync();
    }
}
