using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesWriteOnlyRepository
{
    Task Add(Expense expense);
    /// <summary>
    /// This Function returns TRUE if the delete was successful and FALSE if the expense was not found. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Delete (long id ); 
}
