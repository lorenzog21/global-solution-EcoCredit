using EcoCredit.API.Models;

namespace EcoCredit.API.Repositories;

public interface IUserRepository {
    Task<User?> FindByEmailAsync(string email);
    Task AddAsync(User user);
}
