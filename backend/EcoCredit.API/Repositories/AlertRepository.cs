using Microsoft.EntityFrameworkCore;
using EcoCredit.API.Data;
using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public class AlertRepository : IAlertRepository {
    private readonly AppDbContext _ctx;
    public AlertRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Alert>> GetByCompanyAsync(string companyId, bool activeOnly) {
        var query = _ctx.Alerts
            .Include(a => a.Facility)
            .Where(a => a.Facility!.CompanyId == companyId);

        if (activeOnly)
            query = query.Where(a => !a.Resolved);

        return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
    }

    public async Task AddAsync(Alert alert) {
        _ctx.Alerts.Add(alert);
        await _ctx.SaveChangesAsync();
    }

    public async Task ResolveAsync(string alertId, string note) {
        var alert = await _ctx.Alerts.FindAsync(alertId);
        if (alert != null) {
            alert.Resolved = true;
            alert.ResolvedAt = DateTime.UtcNow;
            alert.ResolvedNote = note;
            await _ctx.SaveChangesAsync();
        }
    }
}
