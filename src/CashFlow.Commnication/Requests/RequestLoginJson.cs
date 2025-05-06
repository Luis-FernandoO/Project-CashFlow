using System.Globalization;

namespace CashFlow.Commnication.Requests;
public class RequestLoginJson
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
