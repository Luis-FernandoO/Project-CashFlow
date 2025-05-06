namespace CashFlow.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ExisteActiveUserWithEmail(string email);
    Task<Entities.User?> GetUserByEmail(string email);

}
