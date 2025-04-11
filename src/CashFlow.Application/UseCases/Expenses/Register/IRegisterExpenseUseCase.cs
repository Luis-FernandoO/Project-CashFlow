using CashFlow.Commnication.Requests;
using CashFlow.Commnication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register;

public interface IRegisterExpenseUseCase
{
    Task<ResponseRegisterExpenseJson> Execute(RequestExpenseJson request);
}
