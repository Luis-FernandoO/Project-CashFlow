using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _expenseReadonly;
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    public DeleteExpenseUseCase(IExpensesReadOnlyRepository expensesReadOnly, IExpensesWriteOnlyRepository repository,IUnitOfWork unitOfWork, ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _expenseReadonly = expensesReadOnly;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var expenses =  await _expenseReadonly.GetById(loggedUser , id);
        
        if (expenses is null)
        {
            throw new NotFoundException(ResourceErrorsMessages.DESPESA_NAO_ENCONTRADA);

        }
        await _repository.Delete(id);
        
        await _unitOfWork.Commit();
    }
}
