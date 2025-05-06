using CashFlow.Commnication.Requests;
using CashFlow.Commnication.Responses;

namespace CashFlow.Application.UseCases.LoginDoLogin;
public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
