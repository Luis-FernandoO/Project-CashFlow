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

    public async Task Delete(long id)
    {
        var result = await _context.Expenses.FindAsync(id);
   
        _context.Expenses.Remove(result!);

    }

    public async Task<List<Expense>> GetAll(User user)
    {
        return await _context.Expenses.AsNoTracking().Where(expense => expense.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id)
    {
        return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(expenses => expenses.Id == id && expenses.UserId == user.Id);
    }
    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user, long id)
    {
        return await _context.Expenses.FirstOrDefaultAsync(expenses => expenses.Id == id && expenses.UserId == user.Id);
    }

    public void Update(Expense expense)
    {
        _context.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(User user,DateOnly date)
    {

        var startDate = new DateTime(year : date.Year,month : date.Month, day: 1);
        
        var daysInMonth = DateTime.DaysInMonth(year : date.Year, month: date.Month);

        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth , hour: 23, minute : 59, second : 59);


        return await _context
            .Expenses
            .AsNoTracking()
            .Where(expense => expense.UserId == user.Id && expense.DateExpense >= startDate && expense.DateExpense <= endDate)
            .OrderBy(x => x.DateExpense)
            .ThenBy(x => x.Title)
            .ToListAsync();

    }

  
}
