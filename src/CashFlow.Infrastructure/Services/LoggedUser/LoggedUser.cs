using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.DataAcess;
using CashFlow.Infrastructure.DataAcess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.Infrastructure.Services.LoggedUser;
public class LoggedUser : ILoggedUser
{
    private readonly CashFlowDbContext _context;
    
    private readonly ITokenProvider _tokenProvider;
    public LoggedUser(CashFlowDbContext context, ITokenProvider tokenProvider)
    {
        _context = context;
        _tokenProvider = tokenProvider; 
    }
    
    public async Task<User> Get()
    {
        string token = _tokenProvider.TokenOnRequest(); 
        
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

       var identifier =  jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        return await 
            _context
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.UserIdentifier == Guid.Parse(identifier));    
    }
}
