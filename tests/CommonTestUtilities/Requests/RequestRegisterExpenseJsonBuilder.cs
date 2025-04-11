using Bogus;
using CashFlow.Commnication.Enums;
using CashFlow.Commnication.Requests;
namespace CommonTestUtilities.Requests;

public class RequestRegisterExpenseJsonBuilder
{
    public static RequestExpenseJson Build()
    {
       return  new Faker<RequestExpenseJson>()
            .RuleFor(r => r.Title, f => f.Commerce.ProductName())
            .RuleFor(r => r.Date, f => f.Date.Past())
            .RuleFor(r => r.PaymentType, f => f.PickRandom<PaymentType>())
            .RuleFor(r => r.Amount, f => f.Random.Decimal(min: 1, max: 1000));
        
    }
}
