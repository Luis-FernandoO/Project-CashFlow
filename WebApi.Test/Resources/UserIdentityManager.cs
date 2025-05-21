using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;

public class UserIdentityManager
{
    private readonly User _user;
    private readonly string _token;
    private readonly string _password;

    public UserIdentityManager(User user, string token, string password)
    {
        _user = user;
        _token = token;
        _password = password;
    }

    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetToken() => _token;
}
