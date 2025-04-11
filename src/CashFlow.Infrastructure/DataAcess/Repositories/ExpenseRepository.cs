using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAcess.Repositories;

internal class ExpenseRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _context;
    public ExpenseRepository(CashFlowDbContext context)
    {
        _context = context;
    }
    public async Task Add(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
    }

    public async Task<bool> Delete(long id)
    {
        var result = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
        if (result is null)
        {
            return false;
        }
        _context.Expenses.Remove(result);

        return true;
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _context.Expenses.ToListAsync();
    }

    public async Task<Expense?> GetById(long id)
    {
        return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(Expense expense)
    {
        _context.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(DateOnly date)
    {

        var startDate = new DateTime(year : date.Year,month : date.Month, day: 1);
        
        var daysInMonth = DateTime.DaysInMonth(year : date.Year, month: date.Month);

        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth , hour: 23, minute : 59, second : 59);


        return await _context
            .Expenses
            .AsNoTracking()
            .Where(x => x.DateExpense.Date >= startDate && x.DateExpense <= endDate)
            .OrderBy(x => x.DateExpense)
            .ThenBy(x => x.Title)
            .ToListAsync();

    }
}
