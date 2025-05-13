using CashFlow.Domain.Repositories;
using Moq;
using System.Text.RegularExpressions;

namespace CommonTestUtilities.Repositories;

public class UnitOfWorkBuilder 
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();
        
        return mock.Object;
    }
}
