using Microsoft.EntityFrameworkCore;
using EcoCredit.API.Data;
using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public class EmissionRepository : IEmissionRepository {
    private readonly AppDbContext _ctx;
    public EmissionRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<EmissionRecord>> GetByFacilityAsync(string facilityId) =>
        await _ctx.EmissionRecords
            .Where(e => e.FacilityId == facilityId)
            .OrderByDescending(e => e.RecordedAt)
            .ToListAsync();

    public async Task<decimal> GetMonthlyTotalAsync(string facilityId) {
        var now = DateTime.UtcNow;
        var firstOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        return await _ctx.EmissionRecords
            .Where(e => e.FacilityId == facilityId && e.RecordedAt >= firstOfMonth)
            .SumAsync(e => e.QuantityTco2e);
    }

    public async Task AddAsync(EmissionRecord record) {
        _ctx.EmissionRecords.Add(record);
        await _ctx.SaveChangesAsync();
    }
}
