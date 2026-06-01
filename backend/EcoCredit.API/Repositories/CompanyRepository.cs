using Microsoft.EntityFrameworkCore;
using EcoCredit.API.Data;
using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public class CompanyRepository : ICompanyRepository {
    private readonly AppDbContext _ctx;
    public CompanyRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Company>> GetAllAsync() =>
        await _ctx.Companies.Where(c => c.Active).ToListAsync();

    public async Task<Company?> GetByIdAsync(string id) =>
        await _ctx.Companies.FindAsync(id);

    public async Task AddAsync(Company company) {
        _ctx.Companies.Add(company);
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(Company company) {
        _ctx.Companies.Update(company);
        await _ctx.SaveChangesAsync();
    }
}
