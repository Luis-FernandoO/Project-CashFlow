using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.Tokens;

public interface IAccesTokenGenerator
{
    string Generate(User user);
}
