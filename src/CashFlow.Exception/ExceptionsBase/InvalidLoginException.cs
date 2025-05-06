using System.Drawing;
using System.Net;

namespace CashFlow.Exception.ExceptionsBase;
public class InvalidLoginException : CashFlowException
{
    public InvalidLoginException() : base("Email ou Senha Inválidos") {}

    public override int StatusCode => (int) HttpStatusCode.Unauthorized; 
    public override List<string> GetErrors()
    {
        return [Message];
    }
}
