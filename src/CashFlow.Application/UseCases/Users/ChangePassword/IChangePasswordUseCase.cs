using CashFlow.Commnication.Requests;

namespace CashFlow.Application.UseCases.Users.ChangePassword;
public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
