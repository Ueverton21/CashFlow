using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class UserRepository : IUserReadOnlyRepository, IUserWriteRepository, IUserUpdateOnlyRepository
{
    private readonly CashFlowDbContext _context;
    public UserRepository(CashFlowDbContext context, IPasswordEncripter encripter)
    {
        _context = context;
    }
    public async Task<bool> ExistsActiveUserWithEmail(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<User> GetById(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task Register(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        _context.Update(user);
    }
}
    