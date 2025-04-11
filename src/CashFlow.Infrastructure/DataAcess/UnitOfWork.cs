using CashFlow.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CashFlow.Infrastructure.DataAcess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly CashFlowDbContext _context;
    public UnitOfWork(CashFlowDbContext context)
    {
        _context = context;
    }
    public async Task Commit() => await _context.SaveChangesAsync();
}
