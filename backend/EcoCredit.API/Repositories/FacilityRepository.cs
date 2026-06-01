using Microsoft.EntityFrameworkCore;
using EcoCredit.API.Data;
using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public class FacilityRepository : IFacilityRepository {
    private readonly AppDbContext _ctx;
    public FacilityRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Facility>> GetByCompanyAsync(string companyId) =>
        await _ctx.Facilities.Where(f => f.CompanyId == companyId && f.Active).ToListAsync();

    public async Task<Facility?> GetByIdAsync(string id) =>
        await _ctx.Facilities.FindAsync(id);

    public async Task AddAsync(Facility facility) {
        _ctx.Facilities.Add(facility);
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(Facility facility) {
        _ctx.Facilities.Update(facility);
        await _ctx.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(string id) {
        var f = await _ctx.Facilities.FindAsync(id);
        if (f != null) {
            f.Active = false;
            await _ctx.SaveChangesAsync();
        }
    }
}
