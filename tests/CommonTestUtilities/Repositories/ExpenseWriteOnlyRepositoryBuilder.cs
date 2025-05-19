using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories;
public class ExpenseWriteOnlyRepositoryBuilder
{
    public static IExpensesWriteOnlyRepository Build()
    {

        var mock = new Mock<IExpensesWriteOnlyRepository>();
        return mock.Object;
    }
}
