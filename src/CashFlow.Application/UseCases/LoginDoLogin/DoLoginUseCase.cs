using CashFlow.Commnication.Requests;
using CashFlow.Commnication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.LoginDoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccesTokenGenerator _accesTokenGenerator;
    public DoLoginUseCase(
        IUserReadOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        IAccesTokenGenerator accesTokenGenerator
        )
    {
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _accesTokenGenerator = accesTokenGenerator; 

    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    
    {
        var user = await _repository.GetUserByEmail(request.Email);
        
        if(user is null)
        {
            throw new InvalidLoginException();
        }
        var passwordHash = _passwordEncripter.Verify(request.Password, user.Password);
        
        if(!passwordHash)
        {
            throw new InvalidLoginException();
        }

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _accesTokenGenerator.Generate(user)
        };
    }
}
