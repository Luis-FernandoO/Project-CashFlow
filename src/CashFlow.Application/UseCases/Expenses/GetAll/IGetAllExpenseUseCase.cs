using CashFlow.Commnication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public interface IGetAllExpenseUseCase
{
    Task<ResponseExpensesJson> Execute();   
}
