using CashFlow.Commnication.Requests;

namespace CashFlow.Application.UseCases.Expenses.Update;

public interface IUpdateExpenseUseCase
{
    Task Execute(long id, RequestExpenseJson request);
}
