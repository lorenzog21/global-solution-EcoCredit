using Microsoft.EntityFrameworkCore;
using EcoCredit.API.Data;
using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public class UserRepository : IUserRepository {
    private readonly AppDbContext _ctx;
    public UserRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<User?> FindByEmailAsync(string email) =>
        await _ctx.Users.FirstOrDefaultAsync(u => u.Email == email && u.Active);

    public async Task AddAsync(User user) {
        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();
    }
}
