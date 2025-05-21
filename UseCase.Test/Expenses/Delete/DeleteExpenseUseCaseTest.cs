using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using FluentAssertions;

namespace UseCase.Test.Expenses.Delete;
public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);
        var act = async () => await useCase.Execute(expense.Id);
        await act.Should().NotThrowAsync();

    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser);
        var act = async () =>  await useCase.Execute(id: 1000);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorsMessages.DESPESA_NAO_ENCONTRADA));
    }

    private DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repositoryWriteOnly = ExpenseWriteOnlyRepositoryBuilder.Build();
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build(); 
        var loggedUser = LoggedUserBuilder.Build(user); 

        return new DeleteExpenseUseCase(repository, repositoryWriteOnly , unitOfWork, loggedUser);
    }
}
