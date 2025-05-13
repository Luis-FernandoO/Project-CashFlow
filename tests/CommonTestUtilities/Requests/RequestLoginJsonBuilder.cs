using Bogus;
using CashFlow.Commnication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(faker => faker.Email, faker => faker.Internet.Email())
            .RuleFor(faker => faker.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
