using CashFlow.Commnication.Requests;
using CashFlow.Commnication.Responses;

namespace CashFlow.Application.UseCases.Users.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}
