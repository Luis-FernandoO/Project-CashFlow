using System.Drawing;

namespace CashFlow.Commnication.Responses;

public class ResponseErrorJson
{
    public List<string> ErrorMessages { get; set; } 

    public ResponseErrorJson(string errorMessage)
    {
        ErrorMessages = [errorMessage];
    }

    public ResponseErrorJson(List<string> erroMessage)
    {
        ErrorMessages = erroMessage;
    }
}
