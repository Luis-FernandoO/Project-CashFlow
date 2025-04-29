using CashFlow.Commnication.Requests;
using DocumentFormat.OpenXml.Drawing.Charts;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CashFlow.Application.UseCases.Users.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("Nome está Vazio");
        RuleFor(user => user.Email).NotEmpty().WithMessage("Email está Vazio").EmailAddress().WithMessage("Email é ínvalido");
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}
