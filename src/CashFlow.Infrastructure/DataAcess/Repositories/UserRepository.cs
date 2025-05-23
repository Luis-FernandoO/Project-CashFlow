using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAcess.Repositories;
internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly CashFlowDbContext _context;
    public UserRepository(CashFlowDbContext context) => _context = context;

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task Delete(User user)
    {
        var userToRemove = await _context.Users.FindAsync(user.Id);
        _context.Users.Remove(userToRemove!);
    }

    public async Task<bool> ExisteActiveUserWithEmail(string email)
    {
        return await _context.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User> GetById(long id)
    {
        return await _context.Users.FirstAsync(user => user.Id == id);  
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }
}
