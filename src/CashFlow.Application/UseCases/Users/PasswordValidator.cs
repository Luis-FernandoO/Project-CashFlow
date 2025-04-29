using CashFlow.Commnication.Requests;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;

namespace CashFlow.Application.UseCases.Users;

public partial class PasswordValidator<T> : PropertyValidator<T, string>
{

    private const string ERROR_MESSAGE = "ErrorMessage";
    public override string Name => "PasswordValidator";
    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ERROR_MESSAGE}}}";
    }

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, "Sua Senha deve conter no mínimo 8 caracteres!");
            return false;
        }
        
        if(password.Length < 8)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, "Sua Senha deve conter no mínimo 8 caracteres!");
            return false;
        }

        if (!UpperCaseLetter().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, "Sua Senha deve conter pelo menos uma letra maiúscula!");
            return false;
        }
        if (!LowerCaseLetter().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, "Sua Senha deve conter pelo menos uma letra maiúscula!");
            return false;
        }

        return true;
    }

    [GeneratedRegex(@"[a-z]+")]
    private static partial Regex LowerCaseLetter();
    [GeneratedRegex(@"[A-Z]+")]
    private static partial Regex UpperCaseLetter();
}
